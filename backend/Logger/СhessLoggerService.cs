using System;
using DataAccess.Abstractions;


public static class ChessLoggerService
{
    private static ILogRepository _logRepository;

    // Метод для инициализации репозитория, вызывается при старте приложения
    public static void Initialize(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    // Статический метод для логирования
    public static async Task Log(string tag, string message)
    {
        await _logRepository.AddLog(tag, message);
    }

    public static string CoordToString((int,int) coords)
    {
        return $"{CoordToChar(coords.Item1, true)}{CoordToChar(coords.Item2, false)}";
    }
    private static char CoordToChar(int coord, bool isLetter)
    {
        if (isLetter)
        {
            if (coord >= 0 && coord <= 7)
            {
                return (char)('a' + coord); 
            }
        }
        else
        {
            if (coord >= 0 && coord <= 7)
            {
                return (char)('1' + coord);
            }
        }

        return '?';
    }
}