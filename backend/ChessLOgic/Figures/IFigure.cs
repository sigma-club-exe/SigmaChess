namespace ChessLogic.Figures;

public interface IFigure
{
    public char Color { get; }
    public FigureType Type { get; }
    public  bool PossibleMove( ref IFigure?[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition);
    protected bool SquareIsUnderAttack( ref IFigure?[][] board,(int,int) square, char pieceColor);
    public bool IsCheckmate(ref IFigure?[][] board, char color);

    public List<(int, int)> GetPossibleMoves(ref IFigure?[][] board, (int, int) currentPos);
    protected (int, int) FindKing(IFigure?[][] board, char kingColor);
}