namespace ChessLogic.Figures;

public class Pawn : Figure
{
    public override bool PossibleMove(ref IFigure?[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1;
        int startY = moveStartPosition.Item2;
        int endX = moveEndPosition.Item1;
        int endY = moveEndPosition.Item2;
        IFigure? figure = board[startX][startY];
        if (figure == null || figure.Type != FigureType.Pawn)
        {
            return false; // Если на начальной позиции нет фигуры или это не пешка
        }

        int direction =
            figure.Color == 'w' ? 1 : -1; // Для белых пешек движение вверх (координаты уменьшаются), для черных вниз
        // Ход вперед на одну клетку
        if (endX == startX + direction && endY == startY && board[endX][endY] == null)
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            var kingPos = FindKing(board, figure.Color);
            if (SquareIsUnderAttack(ref board,kingPos, figure.Color))
            {
                board[startX][startY] = figure;
                board[endX][endY] = null;
                return false;
            }

            return true;
        }

        // Ход вперед на две клетки, если пешка на своей начальной позиции
        bool isStartingPosition = (figure.Color == 'w' && startX == 1) || (figure.Color == 'b' && startX == 6);
        if (isStartingPosition && endX == startX + 2 * direction && (endY == startY) && (board[endX][endY] == null) &&
            (board[startX + direction][startY] == null))
        {
            board[startX][startY] = null;
            board[endX][endY] = figure;
            var kingPos = FindKing(board, figure.Color);
            if (SquareIsUnderAttack(ref board,kingPos, figure.Color))
            {
                board[startX][startY] = figure;
                board[endX][endY] = null;
                return false;
            }

            return true;
        }

        // Взятие фигуры по диагонали
        if (endX == startX + direction && (endY == startY - 1 || endY == startY + 1) && board[endX][endY] != null &&
            board[endX][endY].Color != figure.Color)
        {
            var tempFigure = board[endX][endY];
            board[startX][startY] = null;
            board[endX][endY] = figure;
            var kingPos = FindKing(board, figure.Color);
            if (SquareIsUnderAttack(ref board,kingPos, figure.Color))
            {
                board[startX][startY] = figure;
                board[endX][endY] = tempFigure;
                return false;
            }

            return true;
        }

        return false; // Все другие ходы недопустимы для пешки
    }

    public Pawn(char color) : base(color, FigureType.Pawn)
    {
    }
}