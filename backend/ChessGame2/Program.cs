using System.Collections.Immutable;
using ChessGame2;
using Fleck;
using ChessLogic;

var server = new WebSocketServer("ws://0.0.0.0:8181");

var wsConnections = new List<IWebSocketConnection>();

var wsConnectionsQueue = new Dictionary<string, IWebSocketConnection>();

var games = new Dictionary<string, GameSession>();

var usernames = new Dictionary<IWebSocketConnection, string>();

server.Start(ws =>
{
    ws.OnOpen = () => { wsConnections.Add(ws); };

    ws.OnMessage = async message =>
    {
        if (message.Contains("challenge"))
        {
            var parts = message.Split(" ");
            var gameId = parts[1];
            var username = parts[2];
            usernames[ws] = username;

            if (!wsConnectionsQueue.ContainsKey(gameId)) // no session with such ID yet
            {
                wsConnectionsQueue[gameId] = ws; // player1 init game
            }
            else // player2 accepts invitation by providing same ID
            {
                games[gameId] = new GameSession(new WsChessClient(wsConnectionsQueue[gameId]), new WsChessClient(ws),
                    new Game(), false);
                var currentSession = games[gameId];

                var player1Nick = usernames[currentSession.Player1.PlayerConnection];
                var player2Nick = usernames[currentSession.Player2.PlayerConnection];
                
                var player1Fen = "";
                var player2Fen = "";

                if (currentSession.Player1.Color=='w'){
                    player1Fen = currentSession.GetBoardStateWhite();
                     player2Fen = currentSession.GetBoardStateBlack();
                }else{
                     player2Fen = currentSession.GetBoardStateWhite();
                     player1Fen = currentSession.GetBoardStateBlack();
                }

                currentSession.Player1.PlayerConnection.Send($"GAMESTARTED:{player2Nick}:{player1Fen}:{currentSession.Player1.Color}");
                currentSession.Player2.PlayerConnection.Send($"GAMESTARTED:{player1Nick}:{player2Fen}:{currentSession.Player2.Color}");
                usernames.Remove(currentSession.Player1.PlayerConnection);
                usernames.Remove(currentSession.Player2.PlayerConnection);
            }
        }
        else if (message.Contains("resign"))
        {
            var gameId = message[7..];
            var currentGame = games[gameId];
            wsConnectionsQueue.Remove(gameId);

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
            games.Remove(gameId);
        }
        else if (message.Contains("draw-accepted"))
        {
            var gameId = message[14..];
            var currentGame = games[gameId];
            currentGame.Player1.PlayerConnection.Send("DRAW-ACCEPTED");
            currentGame.Player2.PlayerConnection.Send("DRAW-ACCEPTED");
            wsConnectionsQueue.Remove(gameId);
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
                            currentSession.Player2.PlayerConnection.Send("CHECKMATE:L");
                            currentSession.Player1.PlayerConnection.Send("CHECKMATE:W");
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
                        currentSession.Player2.PlayerConnection.Send("CHECKMATE:W");
                        currentSession.Player1.PlayerConnection.Send("CHECKMATE:L");
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
