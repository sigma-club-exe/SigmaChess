namespace ChessLogic.Figures;

public abstract class Figure : IFigure
{
    protected Figure(char color, FigureType type)
    {
        Color = color;
        Type = type;
    }

    public char Color { get; private set; }
    public FigureType Type { get; protected set; }
    public abstract bool PossibleMove(ref IFigure?[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition);

    public virtual bool KingIsUnderAttack(IFigure?[][] board, (int x, int y) position, char kingColor)
    {
        for (var column = 0; column < 8; column++)
        {
            for (var row = 0; row < 8; row++)
            {
                var figure = board[column][row];
                // Если фигура противника
                if (figure != null && figure.Color != kingColor)
                {
                    // Проверяем, может ли фигура атаковать клетку
                    var tempKing = board[position.x][position.y];
                    if (figure.PossibleMove(ref board, (column, row), position))
                    {
                        figure.PossibleMove(ref board, position, (column, row));
                        board[position.x][position.y] = tempKing;
                        return true; // Клетка под ударом
                    }
                }
            }
        }

        return false;
    }

    public virtual bool KingIsUnderAttack(IFigure?[][] board, char pieceColor)
    {
        (int x, int y) kingPosition = FindKing(board, pieceColor);

        for (var column = 0; column < 8; column++)
        {
            for (var row = 0; row < 8; row++)
            {
                var figure = board[column][row];
                // Если фигура противника
                if (figure != null && figure.Color != pieceColor)
                {
                    var tempPiece = board[kingPosition.x][kingPosition.y];
                    // Проверяем, может ли фигура атаковать клетку
                    if (figure.PossibleMove(ref board, (column, row), kingPosition))
                    {
                        figure.PossibleMove(ref board, kingPosition, (column, row));
                        if (tempPiece != null)
                        {
                            board[kingPosition.x][kingPosition.y] = tempPiece;
                        }

                        return true; // Клетка под ударом
                    }
                }
            }
        }

        return false;
    }


    public virtual List<(int, int)> GetPossibleMoves(ref IFigure?[][] board, (int, int) currentPos)
    {
        List<(int, int)> possibleMoves = new List<(int, int)>();
        int x = currentPos.Item1;
        int y = currentPos.Item2;
        var figure = board[x][y];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var tempFigure = board[i][j];
                if ((i, j) != currentPos && PossibleMove(ref board, currentPos, (i, j)))
                {
                    PossibleMove(ref board, (i, j), currentPos);
                    board[i][j] = tempFigure;
                    possibleMoves.Add((i, j));
                }
            }
        }

        return possibleMoves;
    }

    public virtual bool SquareIsUnderAttack(ref IFigure?[][] board, (int, int) square, char pieceColor)
    {
        for (var column = 0; column < 8; column++)
        {
            for (var row = 0; row < 8; row++)
            {
                var figure = board[column][row];
                // Если фигура противника
                if (figure != null && figure.Color != pieceColor)
                {
                    // Проверяем, может ли фигура атаковать клетку
                    var tempFigure = board[square.Item1][square.Item2];
                    if (figure.PossibleMove(ref board, (column, row), square))
                    {
                        figure.PossibleMove(ref board, square, (column, row));
                        if (tempFigure != null)
                        {
                            board[square.Item1][square.Item2] = tempFigure;
                        }

                        return true; // Клетка под ударом
                    }
                }
            }
        }

        return false;
    }

    public bool IsCheckmate(ref IFigure?[][] board, char color)
    {
        // Шаг 1: Найти короля данного цвета
        (int kingX, int kingY) kingPos = FindKing(board, color);
        var king = board[kingPos.kingX][kingPos.kingY];

        // Шаг 2: Проверить, под шахом ли король
        if (!KingIsUnderAttack(board, kingPos, color))
        {
            return false; // Если король не под шахом, мата нет
        }

        // Шаг 3: Проверить, может ли король сделать ход, чтобы выйти из шаха
        var kingMoves = king!.GetPossibleMoves(ref board, kingPos);
        if (kingMoves.Count >= 1)
        {
            return false;
        }

        // Шаг 4: Проверить, могут ли другие фигуры защитить короля
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                var figure = board[x][y];
                if (figure != null && figure.Color == color)
                {
                    var possibleMoves = figure.GetPossibleMoves(ref board, (x, y));
                    foreach (var move in possibleMoves)
                    {
                        var tempFigure = board[move.Item1][move.Item2];
                        figure.PossibleMove(ref board, (x, y), move);

                        // Если после этого хода король больше не под шахом, мата нет
                        if (!KingIsUnderAttack(board, kingPos, color))
                        {
                            figure.PossibleMove(ref board, move,(x, y));
                            board[move.Item1][move.Item2] =  tempFigure;
                            return false;
                        }
                        figure.PossibleMove(ref board, move,(x, y));
                        board[move.Item1][move.Item2] =  tempFigure;
                    }
                }
            }
        }

        // Если никакой ход не спасает короля от шаха, это мат
        return true;
    }

    public virtual (int, int) FindKing(IFigure?[][] board, char kingColor)
    {
        for (var column = 0; column < 8; column++) // находим союзного короля
        {
            for (var row = 0; row < 8; row++)
            {
                if (board[column][row] != null)
                {
                    if (board[column][row].Type == FigureType.King && board[column][row].Color == kingColor)
                    {
                        return (column, row);
                    }
                }
            }
        }

        return (0, 0);
    }
}