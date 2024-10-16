namespace ChessLogic.Figures;

public class Rook : Figure
{
    public override MoveResult PossibleMove(ref IFigure?[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1; // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2; // Вертикальная координата (строка)

        IFigure? figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.Rook)
        {
            return new MoveResult.Failure(); // Если на начальной позиции нет фигуры или это не ладья
        }

        // Ладья может двигаться только по прямой линии: либо по горизонтали, либо по вертикали
        if (startX != endX && startY != endY)
        {
            return new MoveResult.Failure(); // Ладья не может двигаться по диагонали
        }

        // Проверка пути: должен быть свободен весь путь от старта до конца (без препятствий)
        if (startX == endX) // Вертикальное движение  
        {
            int step = startY < endY ? 1 : -1; // Определяем направление движения
            for (int y = startY + step; y != endY; y += step)
            {
                if (board[startX][y] != null)
                {
                    return new MoveResult.Failure(); // Если на пути есть фигура, ход невозможен
                }
            }
        }
        else if (startY == endY) // Горизонтальное движение
        {
            int step = startX < endX ? 1 : -1; // Определяем направление движения
            for (int x = startX + step; x != endX; x += step)
            {
                if (board[x][startY] != null)
                {
                    return new MoveResult.Failure(); // Если на пути есть фигура, ход невозможен
                }
            }
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
                return new MoveResult.Failure();
            }
            RookDidMove = true;
            return new MoveResult.Success();
        }

        return new MoveResult.Failure(); // Если в конечной клетке фигура того же цвета, ход невозможен
    }
    
    public bool RookDidMove { get; set; }

    public Rook(char color) : base(color, FigureType.Rook)
    {
        Type = FigureType.Rook;
    }
}