using System;
using System.IO;

public static class Logger
{
    private static string _filePath = "C:\\Users\\fyodo.DESKTOP-9A67S1J\\RiderProjects\\SigmaChess\\backend\\Logger\\LogFiles\\Logs.txt";

    /// <summary>
    /// Устанавливает путь к файлу для логирования.
    /// </summary>
    /// <param name="filePath">Путь к файлу для сохранения логов.</param>
    public static void SetFilePath(string filePath)
    {
        _filePath = filePath;

        // Создаем файл, если он не существует
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "=== Лог создан: " + DateTime.Now + " ===\n");
        }
    }

    /// <summary>
    /// Добавляет текст в файл лога.
    /// </summary>
    /// <param name="message">Текст сообщения для добавления в лог.</param>
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

    /// <summary>
    /// Читает и возвращает содержимое лога.
    /// </summary>
    /// <returns>Содержимое лога.</returns>
    public static string ReadLog()
    {
        try
        {
            return File.ReadAllText(_filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения лога: {ex.Message}");
            return string.Empty;
        }
    }
}