using System;
using System.IO;

public static class Logger
{
    private static string _filePath = "/root/SigmaChess/backend/Logger/LogFiles/Logs.txt";

   
    public static void Log(string message)
    {
        try
        {
            // Добавляем сообщение в файл с меткой времени
            string logEntry = $"{DateTime.Now}: {message}";
            File.AppendAllText(_filePath, logEntry + Environment.NewLine);
            Console.WriteLine("Сообщение успешно добавлено в лог.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи в лог: {ex.Message}");
        }
    }
}