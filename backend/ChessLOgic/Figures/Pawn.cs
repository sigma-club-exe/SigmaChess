namespace ChessLogic.Figures;

public class Pawn : Figure
{
    public override MoveResult PossibleMove(ref IFigure?[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1;
        int startY = moveStartPosition.Item2;
        int endX = moveEndPosition.Item1;
        int endY = moveEndPosition.Item2;
        IFigure? figure = board[startX][startY];
        var pieceOnSquare = board[endX][endY];
        if (figure == null || figure.Type != FigureType.Pawn)
        {
            return new MoveResult.Failure(); // Если на начальной позиции нет фигуры или это не пешка
        }

        int direction =
            figure.Color == 'w' ? 1 : -1; // Для белых пешек движение вверх (координаты уменьшаются), для черных вниз
        // Ход вперед на одну клетку
        if (endX == startX + direction && endY == startY && board[endX][endY] == null)
        {
            var kingPos = FindKing(board, figure.Color);
            if (SquareIsUnderAttack(ref board, kingPos, figure.Color))
            {
                return new MoveResult.Failure();
            }
            board[startX][startY] = null;
            board[endX][endY] = figure;
           // var kingPos = FindKing(board, figure.Color);
            if (SquareIsUnderAttack(ref board,kingPos, figure.Color))
            {
                board[startX][startY] = figure;
                board[endX][endY] = null;
                return new MoveResult.Failure();
            }
            if ((figure.Color == 'w' && endX == 7) || (figure.Color == 'b' && endX == 0))
            {
                board[startX][startY] = figure;
                board[endX][endY] = null;
                return new MoveResult.PawnTransformation(figure.Color); // Пешка может двигаться на последнюю горизонталь для превращения
            }

            return new MoveResult.Success();
        }

        // Ход вперед на две клетки, если пешка на своей начальной позиции
        bool isStartingPosition = (figure.Color == 'w' && startX == 1) || (figure.Color == 'b' && startX == 6);
        if (isStartingPosition && endX == startX + 2 * direction && (endY == startY) && (board[endX][endY] == null) &&
            (board[startX + direction][startY] == null))
        {
            var kingPos = FindKing(board, figure.Color);
            if (SquareIsUnderAttack(ref board, kingPos, figure.Color))
            {
                return new MoveResult.Failure();
            }
            board[startX][startY] = null;
            board[endX][endY] = figure;
           // var kingPos = FindKing(board, figure.Color);
            if (SquareIsUnderAttack(ref board,kingPos, figure.Color))
            {
                board[startX][startY] = figure;
                board[endX][endY] = null;
                return new MoveResult.Failure();
            }
            
            Logger.Log("пешка походила");
            return new MoveResult.Success();
        }

        // Взятие фигуры по диагонали
        if (endX == startX + direction && (endY == startY - 1 || endY == startY + 1) && board[endX][endY] != null &&
            board[endX][endY].Color != figure.Color)
        {
            var kingPos = FindKing(board, figure.Color);
            if (SquareIsUnderAttack(ref board, kingPos, figure.Color))
            {
                return new MoveResult.Failure();
            }
            var tempFigure = board[endX][endY];
            board[startX][startY] = null;
            board[endX][endY] = figure;
            // var kingPos = FindKing(board, figure.Color);
            if (SquareIsUnderAttack(ref board,kingPos, figure.Color))
            {
                board[startX][startY] = figure;
                board[endX][endY] = tempFigure;
                return new MoveResult.Failure();
            }
            if ((figure.Color == 'w' && endX == 7) || (figure.Color == 'b' && endX == 0))
            {
                board[startX][startY] = figure;
                board[endX][endY] = tempFigure;
                return new MoveResult.PawnTransformation(figure.Color); // Пешка может взять фигуру на последней горизонтали для превращения
            }

            return new MoveResult.Success();
        }

        return new MoveResult.Failure(); // Все другие ходы недопустимы для пешки
    }
    
    public void TransformTo(FigureType figureType, ref IFigure?[][] board, (int, int) moveStartPosition,
        (int, int) moveEndPosition)
    {
        var figure = board[moveStartPosition.Item1][moveStartPosition.Item2];
        if (figure != null)
        {
            // Сделай каст к такому же типу как и figureType
            switch (figureType)
            {
                case FigureType.Queen:
                    figure = new Queen(figure.Color);
                    break;
                case FigureType.Rook:
                    figure = new Rook(figure.Color);
                    break;
                case FigureType.Bishop:
                    figure = new Bishop(figure.Color);
                    break;
                case FigureType.Knight:
                    figure = new Knight(figure.Color);
                    break;
                default:
                    throw new ArgumentException("Invalid figure type for promotion");
            }

            board[moveStartPosition.Item1][moveStartPosition.Item2] = null;
            board[moveEndPosition.Item1][moveEndPosition.Item2] = figure;
        }
    }

    public Pawn(char color) : base(color, FigureType.Pawn)
    {
    }
}