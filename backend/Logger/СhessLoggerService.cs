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
}