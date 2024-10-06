namespace ChessLogic.Figures;

public class King : Figure
{
    public override bool PossibleMove(ref IFigure?[][] board, (int, int) moveStartPosition, (int, int) moveEndPosition)
    {
        int startX = moveStartPosition.Item1; // Горизонтальная координата (столбец)
        int startY = moveStartPosition.Item2; // Вертикальная координата (строка)
        int endX = moveEndPosition.Item1; // Горизонтальная координата (столбец)
        int endY = moveEndPosition.Item2; // Вертикальная координата (строка)

        IFigure? figure = board[startX][startY];

        if (figure == null || figure.Type != FigureType.King)
        {
            return false; // Если на начальной позиции нет фигуры или это не король
        }

        // Король может двигаться на одну клетку в любом направлении
        int deltaX = Math.Abs(endX - startX);
        int deltaY = Math.Abs(endY - startY);


        // Рокировка (O-O) белых
        if (startX == 0 && startY == 4 && endX == 0 && endY == 6 && !KingDidMove) // Короткая рокировка
        {
            // Проверяем, что ладья находится на позиции (0, 7), и она не двигалась
            IFigure? rook = board[0][7];

            // Приводим к типу Rook, чтобы получить доступ к RookDidMove
            if (rook is Rook castedRook && !castedRook.RookDidMove)
            {
                // Проверяем, что клетки между королем и ладьей свободны и они не под ударом
                if (board[0][5] == null && board[0][6] == null &&
                    !SquareIsUnderAttack(ref board, (0, 5), figure.Color) &&
                    !SquareIsUnderAttack(ref board, (0, 6), figure.Color))
                {
                    // Делаем рокировку: перемещаем короля и ладью
                    board[0][4] = null; // Король покидает исходную позицию
                    board[0][6] = figure; // Король перемещается на новую позицию
                    board[0][7] = null; // Ладья покидает исходную позицию
                    board[0][5] = castedRook; // Ладья становится на новое место

                    KingDidMove = true; // Обновляем флаг движения короля
                    castedRook.RookDidMove = true; // Обновляем флаг движения ладьи

                    return true;
                }
            }
        }

        // Длинная рокировка (O-O-O) белых
        if (endX == 0 && (endY == 2 || endY == 1) && startX == 0 && startY == 4 &&
            !KingDidMove) // Длинная рокировка
        {
            // Проверяем, что ладья находится на позиции (0, 0), и она не двигалась
            IFigure? rook = board[0][0];

            // Приводим к типу Rook, чтобы получить доступ к RookDidMove
            if (rook is Rook castedRook && !castedRook.RookDidMove)
            {
                // Проверяем, что клетки между королем и ладьей свободны и они не под ударом
                if (board[0][1] == null && board[0][2] == null && board[0][3] == null &&
                    !SquareIsUnderAttack(ref board, (0, 1), figure.Color) &&
                    !SquareIsUnderAttack(ref board, (0, 2), figure.Color) &&
                    !SquareIsUnderAttack(ref board, (0, 3), figure.Color))
                {
                    // Делаем рокировку: перемещаем короля и ладью
                    board[0][4] = null; // Король покидает исходную позицию
                    board[0][2] = figure; // Король перемещается на новую позицию
                    board[0][0] = null; // Ладья покидает исходную позицию
                    board[0][3] = castedRook; // Ладья становится на новое место

                    KingDidMove = true; // Обновляем флаг движения короля
                    castedRook.RookDidMove = true; // Обновляем флаг движения ладьи

                    return true;
                }
            }
        }

        // Рокировка (o-o) черных
        if (startX == 7 && startY == 4 && endX == 7 && endY == 6 && !KingDidMove) // Короткая рокировка
        {
            // Проверяем, что ладья находится на позиции (0, 7), и она не двигалась
            IFigure? rook = board[7][7];
            // Приводим к типу Rook, чтобы получить доступ к RookDidMove
            if (rook is Rook castedRook && !castedRook.RookDidMove)
            {
                // Проверяем, что клетки между королем и ладьей свободны и они не под ударом
                if (board[7][5] == null && board[7][6] == null &&
                    !SquareIsUnderAttack(ref board, (7, 5), figure.Color) &&
                    !SquareIsUnderAttack(ref board, (7, 6), figure.Color))
                {
                    // Делаем рокировку: перемещаем короля и ладью
                    board[7][4] = null; // Король покидает исходную позицию
                    board[7][6] = figure; // Король перемещается на новую позицию
                    board[7][7] = null; // Ладья покидает исходную позицию
                    board[7][5] = castedRook; // Ладья становится на новое место

                    KingDidMove = true; // Обновляем флаг движения короля
                    castedRook.RookDidMove = true; // Обновляем флаг движения ладьи
                    return true;
                }
            }
        }

        // Длинная рокировка (o-o-o) черных
        if (endX == 7 && (endY == 2 || endY == 1) && startX == 7 && startY == 4 &&
            !KingDidMove) // Длинная рокировка
        {
            // Проверяем, что ладья находится на позиции (0, 0), и она не двигалась
            IFigure? rook = board[7][0];

            // Приводим к типу Rook, чтобы получить доступ к RookDidMove
            if (rook is Rook castedRook && !castedRook.RookDidMove)
            {
                // Проверяем, что клетки между королем и ладьей свободны и они не под ударом
                if (board[7][1] == null && board[7][2] == null && board[7][3] == null &&
                    !SquareIsUnderAttack(ref board, (7, 1), figure.Color) &&
                    !SquareIsUnderAttack(ref board, (7, 2), figure.Color) &&
                    !SquareIsUnderAttack(ref board, (7, 3), figure.Color))
                {
                    // Делаем рокировку: перемещаем короля и ладью
                    board[7][4] = null; // Король покидает исходную позицию
                    board[7][2] = figure; // Король перемещается на новую позицию
                    board[7][0] = null; // Ладья покидает исходную позицию
                    board[7][3] = castedRook; // Ладья становится на новое место

                    KingDidMove = true; // Обновляем флаг движения короля
                    castedRook.RookDidMove = true; // Обновляем флаг движения ладьи

                    return true;
                }
            }
        }

        if ((deltaX <= 1 && deltaY <= 1) &&
            !(deltaX == 0 && deltaY == 0)) // Движение на 1 клетку по горизонтали, вертикали или диагонали
        {
            // Если конечная клетка пуста или там фигура противника
            if (board[endX][endY] == null || board[endX][endY].Color != figure.Color)
            {
                // проверка то что поле на которое ходит король не находится под ударом вражеских фигур
                if (!SquareIsUnderAttack(ref board, moveEndPosition, figure.Color))
                {
                    board[startX][startY] = null;
                    board[endX][endY] = figure;
                    KingDidMove = true;
                    return true;
                }
            }
        }

        return false; // Любое другое движение недопустимо для короля
    }

    public override (int, int) FindKing(IFigure?[][] board, char kingColor)
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

    public override List<(int, int)> GetPossibleMoves(ref IFigure?[][] board, (int, int) currentPos)
    {
        List<(int, int)> possibleMoves = new List<(int, int)>();
        int x = currentPos.Item1;
        int y = currentPos.Item2;
        var king = board[x][y];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var tempPiece = board[i][j];
                if ((i, j) != currentPos && PossibleMove(ref board, currentPos, (i, j)))
                {
                    // PossibleMove(ref board, (i, j), currentPos);
                    board[i][j] = null;
                    board[x][y] = king;
                    possibleMoves.Add((i, j));
                    if (tempPiece != null)
                    {
                        board[i][j] = tempPiece;
                    }
                }
            }
        }

        return possibleMoves;
    }

    public bool KingDidMove { get; set; }

    public King(char color) : base(color, FigureType.King)
    {
    }
}