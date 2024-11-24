using ChessLogic;
using ChessLogic.Figures;
using Xunit;
using Assert = Xunit.Assert;

namespace ChessGame2.Tests;

public class KnightTest : FigureTest
{
    public KnightTest() : base(new Game())
    {
    }

    [Fact]
    public void Test_WhiteKnightMove_Success()
    {
        var result = RunThroughGame("g1f3");
        Assert.IsType<MoveResult.Success>(result);
        var knight = GetPieceBySquare("f3");
        Assert.NotNull(knight);
        Assert.IsType<Knight>(knight);
        Assert.Equal('w', knight.Color);
    }

    [Fact]
    public void Test_WhiteKnightInvalidMove_Failure()
    {
        var result = RunThroughGame("g1g4");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_BlackKnightMove_Success()
    {
        RunThroughGame("g8f6");
        var knight = GetPieceBySquare("f6");
        Assert.NotNull(knight);
        Assert.IsType<Knight>(knight);
        Assert.Equal('b', knight.Color);
    }

    [Fact]
    public void Test_BlackKnightInvalidMove_Failure()
    {
        var result = RunThroughGame("g8g6");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_KnightCapture_Success()
    {
        RunThroughGame("g1f3 b8c6 e2e4 c6e5");
        var knight = GetPieceBySquare("e5");
        Assert.NotNull(knight);
        Assert.IsType<Knight>(knight);
        Assert.Equal('b', knight.Color);
    }

    [Fact]
    public void Test_KnightCannotMoveToOccupiedSquare_Failure()
    {
        RunThroughGame("b1a3 g1f3 f3d4 d4b5");
        var result = RunThroughGame("b5a3");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }
}