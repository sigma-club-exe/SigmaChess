﻿using System.Text;
using Fleck;
using ChessLogic;

namespace ChessGame2;

public class GameSession
{
    public GameSession(WsChessClient player1, WsChessClient player2, Game boardState, bool botGame)
    {
        Player1 = player1;
        Player2 = player2;
        BoardState = boardState;
        BotGame = botGame;

        var random = new Random();
        if (random.Next(2) == 0)
        {
            Player1.Color = 'w'; // White for Player1
            Player2.Color = 'b'; // Black for Player2
        }
        else
        {
            Player1.Color = 'b'; // Black for Player1
            Player2.Color = 'w'; // White for Player2
        }
    }

    public GameSession(WsChessClient player1, Game boardState, bool botGame)
    {
        Player1 = player1;
        Player2 = new WsChessClient();
        BoardState = boardState;
        BotGame = botGame;

        Player1.Color = 'w';
        Player2.Color = 'b';
    }

    public WsChessClient Player1 { get; set; }
    public WsChessClient Player2 { get; set; }
    public Game BoardState { get; set; }

    public bool BotGame { get; set; }

    public void ApplyMove(string move, WsChessClient player)
    {
        var num = BoardState.CharToCoord(move[1]);
        var letter = BoardState.CharToCoord(move[0]);

        var whitePieceMove = char.IsUpper(
            BoardState.GetFigureSymbol(
                BoardState.Board[num][letter]));
        // we are checking the color of piece on specified cell (upper case means white)

        if ((whitePieceMove && player.Color == 'w') || (!whitePieceMove && player.Color == 'b'))
        {
            var successfulMove = BoardState.DoMove(move);
            if (successfulMove)
            {
                string  checkSquare = string.Empty;
                if (BoardState.Check != (-1, -1))
                {
                    checkSquare =
                        $"{BoardState.CoordToChar(BoardState.Check.Item1, true)}{BoardState.CoordToChar(BoardState.Check.Item2, false)}";
                }
                else
                {
                    checkSquare = "--";
                }

                if (Player1.Color == 'b')
                {
                    Player1.PlayerConnection.Send(
                        $"FEN:{GetBoardStateBlack()}:{Player1.Color}:{GetPlayerCapturedPieces(Player1)}:{GetPlayerCapturedPieces(Player2)}:{checkSquare}:{move}");
                }

                else
                {
                    Player1.PlayerConnection.Send(
                        $"FEN:{GetBoardStateWhite()}:{Player1.Color}:{GetPlayerCapturedPieces(Player1)}:{GetPlayerCapturedPieces(Player2)}:{checkSquare}:{move}");
                }

                if (!BotGame)
                {
                    if (Player2.Color == 'b')
                    {
                        Player2.PlayerConnection.Send(
                            $"FEN:{GetBoardStateBlack()}:{Player2.Color}:{GetPlayerCapturedPieces(Player2)}:{GetPlayerCapturedPieces(Player1)}:{checkSquare}:{move}");
                    }
                    else
                    {
                        Player2.PlayerConnection.Send(
                            $"FEN:{GetBoardStateWhite()}:{Player2.Color}:{GetPlayerCapturedPieces(Player2)}:{GetPlayerCapturedPieces(Player1)}:{checkSquare}:{move}");
                    }
                }
            }
        }
    }

    public void ApplyBotMove(string move) // игрок всегда player1
    {
        var num = BoardState.CharToCoord(move[1]);
        var letter = BoardState.CharToCoord(move[0]);

        var whitePieceMove = char.IsUpper(
            BoardState.GetFigureSymbol(
                BoardState.Board[num][letter]));
        var successfulMove = BoardState.DoMove(move);
        if (successfulMove)
        {
            Player1.PlayerConnection.Send(
                $"FEN:{GetBoardStateWhite()}:{Player1.Color}:{GetPlayerCapturedPieces(Player1)}:{GetPlayerCapturedPieces(Player2)}"); // checkSquare
        }
    }

    public string GetPlayerCapturedPieces(WsChessClient player)
    {
        return BoardState.GetCapturedPieces(player.Color);
    }

    public string GetBoardStateWhite()
    {
        return BoardState.GetBoardAsFEN();
    }

    public string GetBoardStateBlack()
    {
        return BoardState.GetBoardAsFENforBlack();
    }
}