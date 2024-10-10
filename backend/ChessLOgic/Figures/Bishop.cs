namespace ChessLogic.Figures;

public class Bishop : Figure
{
    private Figure _figureImplementation;

    public override bool PossibleMove(ref IFigure?[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1; // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2; // Вертикальная координата (строка)

        IFigure? figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.Bishop)
        {
            return false; // Если на начальной позиции нет фигуры или это не слон
        }

        // Слон может двигаться только по диагоналям, то есть |dx| == |dy|
        int deltaX = Math.Abs(endX - startX);
        int deltaY = Math.Abs(endY - startY);

        if (deltaX != deltaY)
        {
            return false; // Если движение не по диагонали
        }

        // Определяем направление движения
        int stepX = (endX - startX) > 0 ? 1 : -1;
        int stepY = (endY - startY) > 0 ? 1 : -1;

        // Проверяем весь путь на наличие препятствий
        int x = startX + stepX;
        int y = startY + stepY;
        while (x != endX && y != endY)
        {
            if (board[x][y] != null)
            {
                return false; // Если на пути есть фигура, ход невозможен
            }

            x += stepX;
            y += stepY;
        }

        // Если в конечной клетке стоит фигура противника, её можно съесть
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

        return false; // Если в конечной клетке фигура того же цвета, ход невозможен
    }

    public Bishop(char color) : base(color, FigureType.Bishop)
    {
        Type = FigureType.Bishop;
    }
}