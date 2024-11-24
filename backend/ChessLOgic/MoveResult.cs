namespace ChessLogic;

public abstract record MoveResult
{
    private MoveResult()
    {
    }

    public sealed record Success : MoveResult;
    
    public sealed record Failure() : MoveResult;

    public sealed record MoveFailed(string move) : MoveResult;
    
    public sealed record PawnTransformation(char PieceColor) : MoveResult;
}