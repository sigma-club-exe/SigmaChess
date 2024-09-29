using System.Collections.Immutable;
using ChessGame2;
using Fleck;
using ChessLogic;

var server = new WebSocketServer("ws://0.0.0.0:8181");

var wsConnections = new List<IWebSocketConnection>();

var wsConnectionsQueue = new Dictionary<string, IWebSocketConnection>();

var games = new Dictionary<string, GameSession>();

server.Start(ws =>
{
    ws.OnOpen = () => { wsConnections.Add(ws); };

    ws.OnMessage = async message =>
    {
        if (message.Contains("challenge"))
        {
            var gameId = message[10..];
            if (!wsConnectionsQueue.ContainsKey(gameId)) // no session with such ID yet
            {
                wsConnectionsQueue[gameId] = ws; // player1 init game
            }
            else // player2 accepts invitation by providing same ID
            {
                games[gameId] = new GameSession(new WsChessClient(wsConnectionsQueue[gameId]), new WsChessClient(ws),
                    new Game(), false);
                string colorMessage = games[gameId].Player1.Color == 'w' ? "белыми" : "черными";
                wsConnectionsQueue[gameId].Send($"LOGS: Партия {gameId} началась!" +
                                                '\n' + "Сейчас ход белых" + '\n' +
                                                $"Вы играете {colorMessage} фигурами");

                colorMessage = games[gameId].Player2.Color == 'w' ? "белыми" : "черными";
                ws.Send($"LOGS: Партия {gameId} началась!" +
                        '\n' + "Сейчас ход белых" +
                        '\n' + $"Вы играете {colorMessage} фигурами");

                 ws.Send($"GAMEID:{gameId}");
                wsConnectionsQueue[gameId].Send($"GAMEID:{gameId}");
            }
        }
        else if (message.Contains("resign"))
        {
            var gameId = message[7..];
            var currentGame = games[gameId];
            wsConnectionsQueue.Remove(gameId);
            currentGame.Player1.PlayerConnection
                .Send($"LOGS: Партия {gameId} завершилась так как пользователь сдался :(");
            currentGame.Player2.PlayerConnection
                .Send($"LOGS: Партия {gameId} завершилась так как пользователь сдался :(");
            games.Remove(gameId);

            if (ws == currentGame.Player1.PlayerConnection)
            {
                currentGame.Player1.PlayerConnection.Send("RESIGN:L");
                currentGame.Player2.PlayerConnection.Send("RESIGN:W");
            }
            else
            {
                currentGame.Player2.PlayerConnection.Send("RESIGN:L");
                currentGame.Player1.PlayerConnection.Send("RESIGN:W");
            }
            
        }
        else if (message.Contains("draw-accepted"))
        {
            var gameId = message[14..];
            var currentGame = games[gameId];
            currentGame.Player1.PlayerConnection.Send("DRAW-ACCEPTED");
            currentGame.Player2.PlayerConnection.Send("DRAW-ACCEPTED");
            games.Remove(gameId);
        }
        else if (message.Contains("draw"))
        {
            var gameId = message[5..];
            var currentSession = games[gameId];
            if (ws == currentSession.Player1.PlayerConnection)
            {
                currentSession.Player2.PlayerConnection.Send("LOGS:соперник предлагает вам ничью");
                currentSession.Player2.PlayerConnection.Send("DRAW-OFFER");
            }
            
            else
            {
                currentSession.Player1.PlayerConnection.Send("LOGS:соперник предлагает вам ничью");
                currentSession.Player1.PlayerConnection.Send("DRAW-OFFER");
            }
        }
        else if (message.Contains("connected"))
        {
            var parts = message[10..];
            var splitParts = parts.Split(' ');
            var gameId = splitParts[0];
            var username = splitParts[1];
            wsConnectionsQueue[gameId].Send($"CONNECTED:{username}");
            if (ws != wsConnectionsQueue[gameId])
            {
                ws.Send($"CONNECTED:{username}");
            }
        }
        else if (message.Contains(':')) // moves handler
        {
            var parts = message.Split(':');
            var gameId = parts[0];
            var currentMove = parts[1];
            if (games.ContainsKey(gameId))
            {
                var currentSession = games[gameId];

                var currentSessionWhitesTurn = currentSession.BoardState.WhitesTurn;

                if (ws == currentSession.Player1.PlayerConnection &&
                    ((currentSession.Player1.Color == 'w' && currentSessionWhitesTurn) ||
                     (currentSession.Player1.Color == 'b' && !currentSessionWhitesTurn)))
                    // user can move pieces only in his turn 
                    // ходит игрок 1
                {
                    if (currentSession.BotGame)
                    {
                        // ход против бота
                        currentSession.ApplyMove(currentMove, currentSession.Player1);

                        // запрос к апи чтобы сразу получить ход от бота
                        BotEnemy bot = new BotEnemy();
                        var bestMove = await bot.AskForBestMove(currentSession.BoardState.GetBoardAsFullFEN());
                        currentSession.ApplyBotMove(bestMove);
                    }
                    else
                    {
                        currentSession.ApplyMove(currentMove, currentSession.Player1);
                        if (currentSession.BoardState.Checkmate == currentSession.Player2.Color)
                        {
                            currentSession.Player2.PlayerConnection.Send("LOGS:Вы проиграли, вам поставили мат :(");
                            currentSession.Player1.PlayerConnection.Send("LOGS:Вы победили, поставив мат!");
                        }
                    }
                }
                else if (ws == currentSession.Player2.PlayerConnection &&
                         ((currentSession.Player2.Color == 'b' && !currentSessionWhitesTurn) ||
                          (currentSession.Player2.Color == 'w' && currentSessionWhitesTurn)))
                { // ходит игрок 2
                    currentSession.ApplyMove(currentMove, currentSession.Player2);
                    if (currentSession.BoardState.Checkmate == currentSession.Player1.Color)
                    {
                        currentSession.Player1.PlayerConnection.Send("LOGS:Вы проиграли, вам поставили мат :(");
                        currentSession.Player2.PlayerConnection.Send("LOGS:Вы победили, поставив мат!");
                    }
                }
            }
        }
        else if (message.Contains("bot"))
        {
            var gameId = message[4..];
            games[gameId] = new GameSession(new WsChessClient(ws), new Game(), true);
            ws.Send("LOGS:игра с ботом началась!");
        }
    };
});

WebApplication.CreateBuilder(args).Build().Run();
