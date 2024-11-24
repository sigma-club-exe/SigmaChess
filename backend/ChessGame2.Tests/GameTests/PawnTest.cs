using ChessLogic;
using ChessLogic.Figures;
using Xunit;
using Assert = Xunit.Assert;

namespace ChessGame2.Tests;

public class PawnTest : FigureTest
{
    public PawnTest() : base(new Game())
    {
    }

    [Fact]
    public void Test_WhitePawn2SquareMove_Success()
    {
        RunThroughGame("a2a4 b2b4 c2c4 d2d4 e2e4 f2f4 g2g4 h2h4");

        for (int i = 0; i < 8; i++)
        {
            string emptySquare = $"{(char)('a' + i)}2";
            string square = $"{(char)('a' + i)}4";

            var piece = GetPieceBySquare(square);
            var nullPiece = GetPieceBySquare(emptySquare);

            Assert.NotNull(piece);
            Assert.IsType<Pawn>(piece);
            Assert.Null(nullPiece);
        }
    }

    [Fact]
    public void Test_BlackPawn2SquareMove_Success()
    {
        RunThroughGame("a7a5 b7b5 c7c5 d7d5 e7e5 f7f5 g7g5 h7h5");

        for (int i = 0; i < 8; i++)
        {
            string emptySquare = $"{(char)('a' + i)}7";
            string square = $"{(char)('a' + i)}5";

            var piece = GetPieceBySquare(square);
            var nullPiece = GetPieceBySquare(emptySquare);

            Assert.NotNull(piece);
            Assert.IsType<Pawn>(piece);
            Assert.Null(nullPiece);
        }
    }

    [Fact]
    public void Test_WhitePawn1SquareMove_Success()
    {
        RunThroughGame("a2a3 b2b3 c2c3 d2d3 e2e3 f2f3 g2g3 h2h3");

        for (int i = 0; i < 8; i++)
        {
            string emptySquare = $"{(char)('a' + i)}2";
            string square = $"{(char)('a' + i)}3";

            var piece = GetPieceBySquare(square);
            var nullPiece = GetPieceBySquare(emptySquare);

            Assert.NotNull(piece);
            Assert.IsType<Pawn>(piece);
            Assert.Null(nullPiece);
        }
    }

    [Fact]
    public void Test_BlackPawn1SquareMove_Success()
    {
        RunThroughGame("a7a6 b7b6 c7c6 d7d6 e7e6 f7f6 g7g6 h7h6");

        for (int i = 0; i < 8; i++)
        {
            string emptySquare = $"{(char)('a' + i)}7";
            string square = $"{(char)('a' + i)}6";

            var piece = GetPieceBySquare(square);
            var nullPiece = GetPieceBySquare(emptySquare);

            Assert.NotNull(piece);
            Assert.IsType<Pawn>(piece);
            Assert.Null(nullPiece);
        }
    }

    [Fact]
    public void Test_WhitePawnDiagonalCapture_Success()
    {
        RunThroughGame("b7b5 c7c5 d7d5 e7e5 f7f5 g7g5 h7h5");
        RunThroughGame("b5b4 c5c4 d5d4 e5e4 f5f4 g5g4 h5h4");
        RunThroughGame("b4b3 c4c3 d4d3 e4e3 f4f3 g4g3 h4h3");


        RunThroughGame("a2b3 b2c3 c2d3 d2e3 e2f3 f2g3 g2h3");

        for (int i = 0; i < 7; i++)
        {
            string targetSquare = $"{(char)('b' + i)}3";
            string emptySquare = $"{(char)('a' + i)}2";

            var whitePawn = GetPieceBySquare(targetSquare);
            var nullPiece = GetPieceBySquare(emptySquare);

            Assert.NotNull(whitePawn);
            Assert.IsType<Pawn>(whitePawn);
            Assert.Equal('w', whitePawn.Color);
            Assert.Null(nullPiece);
        }
    }

    [Fact]
    public void Test_BlackPawnDiagonalCapture_Success()
    {
        RunThroughGame("b2b4 c2c4 d2d4 e2e4 f2f4 g2g4 h2h4");
        RunThroughGame("b4b5 c4c5 d4d5 e4e5 f4f5 g4g5 h4h5");
        RunThroughGame("b5b6 c5c6 d5d6 e5e6 f5f6 g5g6 h5h6");
        
        RunThroughGame("a7b6 b7c6 c7d6 d7e6 e7f6 f7g6 g7h6");

        for (int i = 0; i < 7; i++)
        {
            string targetSquare = $"{(char)('b' + i)}6";
            string emptySquare = $"{(char)('a' + i)}7";

            var blackPawn = GetPieceBySquare(targetSquare);
            var nullPiece = GetPieceBySquare(emptySquare);

            Assert.NotNull(blackPawn);
            Assert.IsType<Pawn>(blackPawn);
            Assert.Equal('b', blackPawn.Color);
            Assert.Null(nullPiece);
        }
    }

    [Fact]
    public void Test_InvalidPawnJump_Failure()
    {
        var result = RunThroughGame("a2a5");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_PawnInvalidDiagonalCapture_Failure()
    {
        RunThroughGame("a2a3");
        var result = RunThroughGame("a3b4");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_PawnMovesBackward_Failure()
    {
        RunThroughGame("a2a3");
        var result = RunThroughGame("a3a2");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

    [Fact]
    public void Test_PawnMovesToOccupiedSquare_Failure()
    {
        RunThroughGame("a2a3");
        RunThroughGame("b7b5");
        var result = RunThroughGame("a3b5");
        Assert.IsType<MoveResult.MoveFailed>(result);
    }

}