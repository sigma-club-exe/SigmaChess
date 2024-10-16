using ChessLogic.Figures;
using Fleck;

namespace ChessGame2.Tests;

using NUnit.Framework;
using ChessLogic;
using Moq;

public class GameTests
{
    private Game _game;

    [SetUp]
    public void Setup()
    {
        _game = new Game();
    }


    [Test]
    public void Сastling_O_O()
    {
        _game.Board[1][4].PossibleMove(ref _game.Board, (1, 4), (3, 4));
        _game.Board[0][5].PossibleMove(ref _game.Board, (0, 5), (1, 4));
        _game.Board[0][6].PossibleMove(ref _game.Board, (0, 6), (2, 5));
        _game.Board[0][4].PossibleMove(ref _game.Board, (0, 4), (0, 6));

        var whiteKing = _game.Board[0][6];
        Assert.NotNull(whiteKing);
        Assert.AreEqual(FigureType.King, whiteKing.Type);
        Assert.AreEqual('w', whiteKing.Color);
        
        var whiteRook = _game.Board[0][5];
        Assert.NotNull(whiteRook);
        Assert.AreEqual(FigureType.Rook, whiteRook.Type);
        Assert.AreEqual('w', whiteRook.Color);
        
        
        _game.Board[6][4].PossibleMove(ref _game.Board, (6, 4), (4, 4));
        _game.Board[7][5].PossibleMove(ref _game.Board, (7, 5), (6, 4));
        _game.Board[7][6].PossibleMove(ref _game.Board, (7, 6), (5, 5));
        _game.Board[7][4].PossibleMove(ref _game.Board, (7, 4), (7, 6));
        
        var blackKing = _game.Board[7][6];
        Assert.NotNull(blackKing);
        Assert.AreEqual(FigureType.King, blackKing.Type);
        Assert.AreEqual('b', blackKing.Color);
        
        var blackRook = _game.Board[7][5];
        Assert.NotNull(blackRook);
        Assert.AreEqual(FigureType.Rook, blackRook.Type);
        Assert.AreEqual('b', blackRook.Color);
    }

    [Test]
    public void Castling_O_O_O()
    {
        _game.Board[1][3].PossibleMove(ref _game.Board, (1, 3), (3, 3));
        _game.Board[0][2].PossibleMove(ref _game.Board, (0, 2), (2, 4));
        _game.Board[0][3].PossibleMove(ref _game.Board, (0, 3), (1, 3));
        _game.Board[0][1].PossibleMove(ref _game.Board, (0, 1), (2, 0));
        _game.Board[0][4].PossibleMove(ref _game.Board, (0, 4), (0, 2));
        
        var whiteKing = _game.Board[0][2]; 
        Assert.NotNull(whiteKing);
        Assert.AreEqual(FigureType.King, whiteKing.Type);
        Assert.AreEqual('w', whiteKing.Color);
        
        var whiteRook = _game.Board[0][3]; 
        Assert.NotNull(whiteRook);
        Assert.AreEqual(FigureType.Rook, whiteRook.Type);
        Assert.AreEqual('w', whiteRook.Color);
        
        _game.Board[6][3].PossibleMove(ref _game.Board, (6, 3), (4, 3));
        _game.Board[7][2].PossibleMove(ref _game.Board, (7, 2), (5, 4));
        _game.Board[7][3].PossibleMove(ref _game.Board, (7, 3), (6, 3));
        _game.Board[7][1].PossibleMove(ref _game.Board, (7, 1), (5, 0));
        _game.Board[7][4].PossibleMove(ref _game.Board, (7, 4), (7, 2));
        
        var blackKing = _game.Board[7][2]; 
        Assert.NotNull(blackKing);
        Assert.AreEqual(FigureType.King, blackKing.Type);
        Assert.AreEqual('b', blackKing.Color);

        var blackRook = _game.Board[7][3];
        Assert.NotNull(blackRook);
        Assert.AreEqual(FigureType.Rook, blackRook.Type);
        Assert.AreEqual('b', blackRook.Color);
    }

    [Test]
    public void Pawn_Move()
    {
        _game.Board[1][3].PossibleMove(ref _game.Board, (1, 3), (3, 3));
        
        var whitePawn = _game.Board[3][3];
        
        Assert.NotNull(whitePawn);
    }
}