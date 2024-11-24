using ChessLogic.Figures;
using NUnit.Framework;
using ChessLogic;
using Moq;

namespace ChessGame2.Tests;

public class GameTests
{
    private Game _game;

    [SetUp]
    public void Setup()
    {
        _game = new Game();
    }

    public void RunThroughGame(string movesLine)
    {
        var splitedMoves = movesLine.Split(' ');
        foreach (var move in splitedMoves)
        {
            _game.DoMove(move);
        }
    }

    public IFigure? GetPieceBySquare(string square)
    {
        return _game.GetPieceBySquare(square);
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

    [Test]
    public void Pawn_TransformationResult()
    {
        _game.Board[1][3].PossibleMove(ref _game.Board, (1, 3), (3, 3));
        _game.Board[3][3].PossibleMove(ref _game.Board, (3, 3), (4, 3));
        _game.Board[4][3].PossibleMove(ref _game.Board, (4, 3), (5, 3));
        _game.Board[5][3].PossibleMove(ref _game.Board, (5, 3), (6, 4));
        var pawnTransResult = _game.Board[6][4].PossibleMove(ref _game.Board, (6, 4), (7, 5));

        var whitePawn = _game.Board[6][4];

        Assert.NotNull(whitePawn);
        Assert.AreEqual(FigureType.Pawn, whitePawn.Type);
        Assert.AreEqual('w', whitePawn.Color);
        Assert.AreEqual(pawnTransResult, new MoveResult.PawnTransformation('w'));
    }

    [Test]
    public void Test_KidsCheckmate_Success()
    {
        RunThroughGame(
            "e2e4 e7e5 f1c4 b8c6 d1h5 g8f6 h5f7");

        var f7WhiteQueen = GetPieceBySquare("f7");
        var e8BlackKing = GetPieceBySquare("e8");
        
        Assert.NotNull(f7WhiteQueen);
        Assert.That(FigureType.Queen, Is.EqualTo(f7WhiteQueen.Type));
        Assert.That('w', Is.EqualTo(f7WhiteQueen.Color));
        
        Assert.NotNull(null != e8BlackKing);
        Assert.That(FigureType.King, Is.EqualTo(e8BlackKing.Type));
        Assert.That('b', Is.EqualTo(e8BlackKing.Color));
    }

    [Test]
    public void Test_DoubleCheck_Failure()
    {
        RunThroughGame("e2e4 e7e5 e1e2 e8e7 e2e3 e7e6 g1f3 g8f6 f3g5 f6g4");
    }
}