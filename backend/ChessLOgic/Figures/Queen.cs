namespace ChessLogic.Figures;

public class Queen : Figure
{
    public override bool PossibleMove(ref IFigure?[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1; // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2; // Вертикальная координата (строка)

        IFigure? figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.Queen)
        {
            return false; // Если на начальной позиции нет фигуры или это не ферзь
        }

        // Ферзь может двигаться как по диагонали, так и по горизонтали/вертикали
        int deltaX = Math.Abs(endX - startX);
        int deltaY = Math.Abs(endY - startY);

        // Проверка: если ход по диагонали 
        if (deltaX == deltaY)
        {
            // Логика для движения по диагонали (как у слона)
            int stepX = (endX - startX) > 0 ? 1 : -1;
            int stepY = (endY - startY) > 0 ? 1 : -1;

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

            // Если конечная клетка пуста или там фигура противника, ход возможен
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

        // Проверка: если ход по горизонтали или вертикали
        if (startX == endX || startY == endY)
        {
            // Логика для движения по прямой линии (как у ладьи)
            if (startX == endX) // Вертикальное движение
            {
                int stepY = startY < endY ? 1 : -1;
                for (int y = startY + stepY; y != endY; y += stepY)
                {
                    if (board[startX][y] != null)
                    {
                        return false; // Если на пути есть фигура, ход невозможен
                    }
                }
            }
            else if (startY == endY) // Горизонтальное движение
            {
                int stepX = startX < endX ? 1 : -1;
                for (int x = startX + stepX; x != endX; x += stepX)
                {
                    if (board[x][startY] != null)
                    {
                        return false; // Если на пути есть фигура, ход невозможен
                    }
                }
            }

            // Если конечная клетка пуста или там фигура противника, ход возможен
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

        return false; // Все другие ходы недопустимы для ферзя
    }

    public Queen(char color) : base(color, FigureType.Queen)
    {
    }
}