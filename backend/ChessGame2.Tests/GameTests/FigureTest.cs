using ChessLogic;
using ChessLogic.Figures;

namespace ChessGame2.Tests;

public abstract class FigureTest
{
    private readonly Game _game;
    
    protected FigureTest(Game game)
    {
        _game = game;
    }
    
    public  MoveResult RunThroughGame(string movesLine)
    {
        var splitedMoves = movesLine.Split(' ');
        foreach (var move in splitedMoves)
        {
           var moveResult =  _game.DoMove(move);
           if (moveResult is MoveResult.MoveFailed)
           {
               return new MoveResult.MoveFailed(move);
           }
        }

        return new MoveResult.Success();
    }
    
    public IFigure? GetPieceBySquare(string square)
    {
        return _game.GetPieceBySquare(square);
    }
}