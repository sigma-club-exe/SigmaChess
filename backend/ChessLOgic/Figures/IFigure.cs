namespace ChessLogic.Figures;

public interface IFigure
{
    public char Color { get; }
    public FigureType Type { get; set; }
    public  MoveResult PossibleMove( ref IFigure?[][] board,(int,int) moveStartPosition, (int,int) moveEndPosition);
    protected bool SquareIsUnderAttack( ref IFigure?[][] board,(int,int) square, char pieceColor);
    public bool IsCheckmate(ref IFigure?[][] board, char color);

    public (int, int) IsCheck(ref IFigure?[][] board);

    public List<(int, int)> GetPossibleMoves(ref IFigure?[][] board, (int, int) currentPos);
    
    protected (int, int) FindKing(IFigure?[][] board, char kingColor);
}