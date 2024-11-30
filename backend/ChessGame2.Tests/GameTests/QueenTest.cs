using ChessLogic;
using ChessLogic.Figures;
using Xunit;
using Assert = Xunit.Assert;

namespace ChessGame2.Tests;

public class QueenTest : FigureTest
{
    public QueenTest() : base(new Game())
    {
    }

    [Fact]
    public void Test_WhiteQueenMove_Success()
    {
        RunThroughGame("e2e4");
        var moveResult = RunThroughGame("d1h5");
        var whiteQueen = GetPieceBySquare("h5");
        
        Assert.NotNull(whiteQueen);
        Assert.IsType<Queen>(whiteQueen);
        Assert.Equal('w', whiteQueen.Color);
        Assert.IsType<MoveResult.Success>(moveResult);
    }

    [Fact]
    public void Test_BlackQueenMove_Success()
    {
        RunThroughGame("e7e5");
        var moveResult = RunThroughGame("d8h4");
        var blackQueen = GetPieceBySquare("h4");
        
        Assert.NotNull(blackQueen);
        Assert.IsType<Queen>(blackQueen);
        Assert.Equal('b', blackQueen.Color);
        Assert.IsType<MoveResult.Success>(moveResult);
    }

    [Fact]
    public void Test_WhiteQueenInvalidMove_Failure()
    {
        var result = RunThroughGame("d1d3");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_BlackQueenInvalidMove_Failure()
    {
        var result = RunThroughGame("d8d6");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_BlackQueenCapture_Success()
    {
        RunThroughGame("e7e5 d8g5");
        var moveResult = RunThroughGame("g5g2");
        var blackQueen = GetPieceBySquare("g2");
        
        Assert.NotNull(blackQueen);
        Assert.IsType<Queen>(blackQueen);
        Assert.Equal('b', blackQueen.Color);
        Assert.IsType<MoveResult.Success>(moveResult);
    }

    [Fact]
    public void Test_BlackQueenCannotMoveToOccupiedSquare_Failure()
    {
        var result = RunThroughGame("d8d7");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }
}