using Microsoft.Extensions.Configuration;

namespace Logger
{
    public static class FileLogger
    {
        private static readonly string _logFilePath;

        static FileLogger()
        {
            // Создание конфигурации
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("path.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            // Чтение пути к файлу логов из конфигурации
            _logFilePath = configuration["Logging:LogFilePath"];

            // Проверка и создание директории, если она не существует
            var logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Path.IsPathRooted(logDirectory))
            {
                logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logDirectory);
            }
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Если путь относительный, преобразуем его в абсолютный
            if (!Path.IsPathRooted(_logFilePath))
            {
                _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _logFilePath);
            }
        }

        public static void Log(string message)
        {
            try
            {
                string logMessage = $"{DateTime.Now} - {message}";
                File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Обработка исключений при логировании
            }
        }
    }
}