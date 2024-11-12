namespace Logger
{
    public static class FileLogger
    {
        private static readonly string _logFilePath;

        static FileLogger()
        {
            // Установка пути к файлу логов (если папка LogFiles существует в корне проекта)
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles", "Logs.txt");

            // Проверка и создание директории, если она не существует
            var logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public static void Log(string message)
        {
            string logMessage = $"{DateTime.Now} - {message}";
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }
    }
}