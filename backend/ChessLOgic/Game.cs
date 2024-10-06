using System;
using System.Text;
using ChessLogic.Figures;
using Microsoft.VisualBasic;

namespace ChessLogic;

public class Game
{
    public Game()
    {
        // Инициализация доски
        Board = new Figure[8][];
        for (int i = 0; i < 8; i++)
        {
            Board[i] = new Figure[8];
        }

        // Расстановка белых фигур
        Board[0][0] = new Rook('w'); // Ладья
        Board[0][1] = new Knight('w'); // Конь
        Board[0][2] = new Bishop('w'); // Слон
        Board[0][3] = new Queen('w'); // Ферзь
        Board[0][4] = new King('w'); // Король
        Board[0][5] = new Bishop('w'); // Слон
        Board[0][6] = new Knight('w'); // Конь
        Board[0][7] = new Rook('w'); // Ладья
        for (int i = 0; i < 8; i++)
        {
            Board[1][i] = new Pawn('w'); // Пешки на второй линии
        }

        for (int i = 2; i < 6; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Board[i][j] = null;
            }
        }

        // Расстановка черных фигур
        Board[7][0] = new Rook('b'); // Ладья
        Board[7][1] = new Knight('b'); // Конь
        Board[7][2] = new Bishop('b'); // Слон
        Board[7][3] = new Queen('b'); // Ферзь
        Board[7][4] = new King('b'); // Король
        Board[7][5] = new Bishop('b'); // Слон
        Board[7][6] = new Knight('b'); // Конь
        Board[7][7] = new Rook('b'); // Ладья
        for (int i = 0; i < 8; i++)
        {
            Board[6][i] = new Pawn('b'); // Пешки на седьмой линии
        }

        WhitesTurn = true;

        White_O_O = true;
        White_O_O_O = true;
        Black_O_O = true;
        Black_O_O_O = true;
        Checkmate = '-';
    }

    public IFigure?[][] Board;

    public bool WhitesTurn { get; set; }

    public bool White_O_O { get; set; }
    public bool White_O_O_O { get; set; }
    public bool Black_O_O { get; set; }
    public bool Black_O_O_O { get; set; }

    public char Checkmate { get; set; }

    public string GetCapturedPieces(char color)
    {
        // Массив для подсчета недостающих фигур (0: Pawn, 1: Knight, 2: Bishop, 3: Rook, 4: Queen)
        short[] capturedPieces = new short[5]; // Начальные значения все нули

        // Проход по всей доске и подсчет присутствующих фигур
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var curSquare = Board[i][j];
                if (curSquare != null && curSquare.Color != color) // Считаем только вражеские фигуры
                {
                    switch (curSquare.Type)
                    {
                        case FigureType.Pawn:
                            capturedPieces[0]++; // Пешки
                            break;
                        case FigureType.Knight:
                            capturedPieces[1]++; // Кони
                            break;
                        case FigureType.Bishop:
                            capturedPieces[2]++; // Слоны
                            break;
                        case FigureType.Rook:
                            capturedPieces[3]++; // Ладьи
                            break;
                        case FigureType.Queen:
                            capturedPieces[4]++; // Ферзь
                            break;
                    }
                }
            }
        }

        // Для хранения строки с съеденными фигурами
        StringBuilder capturedPiecesAsString = new StringBuilder();

        // Теперь добавляем отсутствующие фигуры в строку захваченных
        for (int j = 0; j < 8 - capturedPieces[0]; j++)
        {
            capturedPiecesAsString.Append('p'); // Пешки
        }

        for (int j = 0; j < 2 - capturedPieces[1]; j++)
        {
            capturedPiecesAsString.Append('n'); // Кони
        }

        for (int j = 0; j < 2 - capturedPieces[2]; j++)
        {
            capturedPiecesAsString.Append('b'); // Слоны
        }

        for (int j = 0; j < 2 - capturedPieces[3]; j++)
        {
            capturedPiecesAsString.Append('r'); // Ладьи
        }

        if (capturedPieces[4] < 1)
        {
            capturedPiecesAsString.Append('q'); // Ферзь
        }

        if (color == 'b')
        {
            return capturedPiecesAsString.ToString().ToUpper();
        }

        return capturedPiecesAsString.ToString();
    }

    public bool DoMove(string move)
    {
        try
        {
            // Преобразуем ход (например, e2e4) в координаты на доске
            (int, int) moveStartCoords = (CharToCoord(move[1]), CharToCoord(move[0]));
            (int, int) moveEndCoords = (CharToCoord(move[3]), CharToCoord(move[2]));

            // Проверяем, есть ли фигура на начальной позиции
            var figure = Board[moveStartCoords.Item1][moveStartCoords.Item2];
            if (figure == null)
            {
                return false;
            }

            // Выполняем ход
            IFigure? tempfigure = Board[moveStartCoords.Item1][moveStartCoords.Item2];
            if (figure.PossibleMove(ref Board, moveStartCoords, moveEndCoords))
            {
                if (Board[moveEndCoords.Item1][moveEndCoords.Item2]
                    .IsCheckmate(ref Board, figure.Color == 'w' ? 'b' : 'w'))
                {
                    Checkmate = figure.Color == 'w' ? 'b' : 'w';
                }


                if (tempfigure.Type == FigureType.King)
                {
                    if (tempfigure.Color == 'w')
                    {
                        White_O_O = false;
                        White_O_O_O = false;
                    }
                    else
                    {
                        Black_O_O = false;
                        Black_O_O_O = false;
                    }
                }
                else if (tempfigure.Type == FigureType.Rook)
                {
                    // todo: доделать логику рокировки
                }


                WhitesTurn = !WhitesTurn;
                return true;
            }
        }
        catch (Exception ex)
        {
            return false; // Возвращаем false при любой ошибке
        }

        return false;
    }

    public int CharToCoord(char c)
    {
        if (char.IsLetter(c))
        {
            var test1 = c - 'a';
            return c - 'a'; // Преобразуем буквы 'a'-'h' в индексы 0-7
        }

        if (char.IsDigit(c))
        {
            var test2 = (c - '0' - 1);
            return (c - '0' - 1); // Преобразуем цифры '1'-'8' в индексы 7-0
        }

        throw new ArgumentException("Некорректный символ для шахматных координат");
    }

    public string GetBoardAsFEN()
    {
        var sb = new StringBuilder();

        for (int x = 7; x >= 0; x--) // от 8-й линии к 1-й
        {
            int emptySquares = 0;

            for (int y = 0; y < 8; y++)
            {
                if (Board[x][y] == null)
                {
                    emptySquares++; // увеличиваем счетчик пустых клеток
                }
                else
                {
                    if (emptySquares > 0)
                    {
                        sb.Append(emptySquares); // добавляем число пустых клеток
                        emptySquares = 0;
                    }

                    sb.Append(GetFigureSymbol(Board[x][y])); // добавляем символ фигуры
                }
            }

            if (emptySquares > 0)
            {
                sb.Append(emptySquares); // добавляем оставшиеся пустые клетки в строке
            }

            if (x > 0)
            {
                sb.Append('/'); // разделитель между строками доски
            }
        }

        return sb.ToString();
    }

    public string GetBoardAsFENforBlack()
    {
        var sb = new StringBuilder();

        for (int x = 0; x < 8; x++) // от 8-й линии к 1-й
        {
            int emptySquares = 0;

            for (int y = 7; y >= 0; y--)
            {
                if (Board[x][y] == null)
                {
                    emptySquares++; // увеличиваем счетчик пустых клеток
                }
                else
                {
                    if (emptySquares > 0)
                    {
                        sb.Append(emptySquares); // добавляем число пустых клеток
                        emptySquares = 0;
                    }

                    sb.Append(GetFigureSymbol(Board[x][y])); // добавляем символ фигуры
                }
            }

            if (emptySquares > 0)
            {
                sb.Append(emptySquares); // добавляем оставшиеся пустые клетки в строке
            }

            if (x < 7)
            {
                sb.Append('/'); // разделитель между строками доски
            }
        }

        return sb.ToString();
    }

    public string GetBoardAsFullFEN()
    {
        char currentTurn = WhitesTurn ? 'w' : 'b';
        bool whiteCanCastleKingside = White_O_O;
        bool whiteCanCastleQueenside = White_O_O_O;
        bool blackCanCastleKingside = Black_O_O;
        bool blackCanCastleQueenside = Black_O_O_O;
        string enPassantTargetSquare = "-";
        int halfMoveClock = 2;
        int fullMoveNumber = 2;
        var sb = new StringBuilder();

        // 1. Генерация положения фигур на доске (как в предыдущем методе)
        for (int x = 7; x >= 0; x--) // от 8-й линии к 1-й
        {
            int emptySquares = 0;

            for (int y = 0; y < 8; y++)
            {
                if (Board[x][y] == null)
                {
                    emptySquares++; // увеличиваем счетчик пустых клеток
                }
                else
                {
                    if (emptySquares > 0)
                    {
                        sb.Append(emptySquares); // добавляем число пустых клеток
                        emptySquares = 0;
                    }

                    sb.Append(GetFigureSymbol(Board[x][y])); // добавляем символ фигуры
                }
            }

            if (emptySquares > 0)
            {
                sb.Append(emptySquares); // добавляем оставшиеся пустые клетки в строке
            }

            if (x > 0)
            {
                sb.Append('/'); // разделитель между строками доски
            }
        }

        // 2. Добавление информации о ходе
        sb.Append(' ').Append(currentTurn); // текущий ход: 'w' или 'b'

        // 3. Возможность рокировки
        sb.Append(' ');
        if (whiteCanCastleKingside) sb.Append('K');
        if (whiteCanCastleQueenside) sb.Append('Q');
        if (blackCanCastleKingside) sb.Append('k');
        if (blackCanCastleQueenside) sb.Append('q');
        if (!whiteCanCastleKingside && !whiteCanCastleQueenside &&
            !blackCanCastleKingside && !blackCanCastleQueenside)
        {
            sb.Append('-'); // если ни одна рокировка не возможна
        }

        // 4. Взятие на проходе
        sb.Append(' ').Append(string.IsNullOrEmpty(enPassantTargetSquare) ? "-" : enPassantTargetSquare);

        // 5. Счётчик полуходов
        sb.Append(' ').Append(halfMoveClock);

        // 6. Номер полного хода
        sb.Append(' ').Append(fullMoveNumber);

        return sb.ToString();
    }


    // Метод для отображения символов фигур
    public char GetFigureSymbol(IFigure? figure)
    {
        if (figure == null)
        {
            return '-';
        }

        switch (figure.Type)
        {
            case FigureType.Pawn:
                return figure.Color == 'w' ? 'P' : 'p';
            case FigureType.King:
                return figure.Color == 'w' ? 'K' : 'k';
            case FigureType.Queen:
                return figure.Color == 'w' ? 'Q' : 'q';
            case FigureType.Bishop:
                return figure.Color == 'w' ? 'B' : 'b';
            case FigureType.Knight:
                return figure.Color == 'w' ? 'N' : 'n';
            case FigureType.Rook:
                return figure.Color == 'w' ? 'R' : 'r';
            default:
                return '-';
        }
    }
}