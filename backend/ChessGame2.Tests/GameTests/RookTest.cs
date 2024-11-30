using ChessLogic;
using ChessLogic.Figures;
using Xunit;
using Assert = Xunit.Assert;

namespace ChessGame2.Tests;

public class RookTest : FigureTest
{
    public RookTest() : base(new Game())
    {
    }

    [Fact]
    public void Test_WhiteRookMove_Success()
    {
        RunThroughGame("a2a4 a1a3 h2h4 h1h3");
        
        var blackSquaredRook = GetPieceBySquare("a3");
        var whiteSquaredRook = GetPieceBySquare("h3");
        
        Assert.NotNull(blackSquaredRook);
        Assert.IsType<Rook>(blackSquaredRook);
        Assert.Equal('w', blackSquaredRook.Color);
        Assert.NotNull(whiteSquaredRook);
        Assert.IsType<Rook>(whiteSquaredRook);
        Assert.Equal('w', whiteSquaredRook.Color);
    }

    [Fact]
    public void Test_WhiteRookInvalidMove_Failure()
    {
        var result = RunThroughGame("a1a2");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_BlackRookMove_Success()
    {
        RunThroughGame("h7h5 h8h6");
        var rook = GetPieceBySquare("h6");
        Assert.NotNull(rook);
        Assert.IsType<Rook>(rook);
        Assert.Equal('b', rook.Color);
    }

    [Fact]
    public void Test_BlackRookInvalidMove_Failure()
    {
        RunThroughGame("h7h5 h8h6");
        var result = RunThroughGame("h6f7");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_RookCapture_Success()
    {
        RunThroughGame("a2a4 d7d6 a1a3 c8h3 a3h3");
        var rook = GetPieceBySquare("h3");
        Assert.NotNull(rook);
        Assert.IsType<Rook>(rook);
        Assert.Equal('w', rook.Color);
    }

    [Fact]
    public void Test_RookCannotMoveToOccupiedSquare_Failure()
    {
        RunThroughGame("a2a4");
        var result = RunThroughGame("a1a4");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_RookBlockedByPiece_Failure()
    {
        RunThroughGame("a1a3");
        var result = RunThroughGame("a3a5");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }
}