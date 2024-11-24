using ChessLogic;
using ChessLogic.Figures;
using Xunit;
using Assert = Xunit.Assert;

namespace ChessGame2.Tests;

public class BishopTest : FigureTest
{
    public BishopTest() : base(new Game())
    {
    }

    [Fact]
    public void Test_WhiteBishopMove_Success()
    {
        RunThroughGame("e2e4 d2d4 f1c4 c1g5");
        var whiteBishop = GetPieceBySquare("c4");
        var blackBishop = GetPieceBySquare("g5");
        
        Assert.NotNull(whiteBishop);
        Assert.IsType<Bishop>(whiteBishop);
        Assert.Equal('w', whiteBishop.Color);
        Assert.NotNull(blackBishop);
        Assert.IsType<Bishop>(blackBishop);
        Assert.Equal('w', blackBishop.Color);
    }

    [Fact]
    public void Test_WhiteBishopInvalidMove_Failure()
    {
        RunThroughGame("e2e4 f1e3");
        var result = RunThroughGame("f1e3");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_BlackBishopMove_Success()
    {
        RunThroughGame("e7e5 d7d5 f8c5 c8g4");
        var blackSquaredBishop = GetPieceBySquare("c5");
        var whiteSquaredBishop = GetPieceBySquare("g4");
        
        Assert.NotNull(blackSquaredBishop);
        Assert.IsType<Bishop>(blackSquaredBishop);
        Assert.Equal('b', blackSquaredBishop.Color);
        
        Assert.NotNull(whiteSquaredBishop);
        Assert.IsType<Bishop>(whiteSquaredBishop);
        Assert.Equal('b', whiteSquaredBishop.Color);
    }

    [Fact]
    public void Test_BlackBishopInvalidMove_Failure()
    {
        RunThroughGame("e7e5 f8e6");
        var result = RunThroughGame("f8e6");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_BishopCapture_Success()
    {
        RunThroughGame("e2e4 e7e5 f1c4 d7d6 c4f7");
        var bishop = GetPieceBySquare("f7");
        Assert.NotNull(bishop);
        Assert.IsType<Bishop>(bishop);
        Assert.Equal('w', bishop.Color);
    }

    [Fact]
    public void Test_BishopCannotMoveToOccupiedSquare_Failure()
    {
        RunThroughGame("e2e4 f1c4 d2d3");
        var result = RunThroughGame("c4d3");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_BishopBlockedByPiece_Failure()
    {
        RunThroughGame("e2e4 d2d3 f1b5");
        var result = RunThroughGame("f1b5");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }
}