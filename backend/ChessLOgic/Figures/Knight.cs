namespace ChessLogic.Figures;

public class Knight : Figure
{

    public override bool PossibleMove(ref IFigure?[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1;     // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2;     // Вертикальная координата (строка)

        IFigure? figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.Knight)
        {
            return false; // Если на начальной позиции нет фигуры или это не конь
        }

        // Варианты возможных движений коня
        int[] dx = { 2, 2, -2, -2, 1, 1, -1, -1 };
        int[] dy = { 1, -1, 1, -1, 2, -2, 2, -2 };

        for (int i = 0; i < 8; i++)
        {
            int newX = startX + dx[i];
            int newY = startY + dy[i];

            if (newX == endX && newY == endY)
            {
                // Если клетка пуста или там фигура противника, ход возможен
                if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
                {
                    var tempPiece = board[endX][endY];
                    board[startX][startY] = null;
                    board[endX][endY] = figure;
                    var kingPos = FindKing(board, figure.Color);
                    if (SquareIsUnderAttack(ref board,kingPos, figure.Color))
                    {
                        board[startX][startY] = figure;
                        board[endX][endY] = tempPiece;
                        return false;
                    }
                    return true;
                }
            }
        }
        
        return false; // Если ни одно из возможных движений коня не подходит
    }
    
    public Knight(char color): base(color,FigureType.Knight)
    {
        Type = FigureType.Knight;
    }
}