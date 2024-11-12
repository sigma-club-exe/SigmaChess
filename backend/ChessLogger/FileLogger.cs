using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ChessLogger;

public static class FileLogger
{
    private static readonly string _logFilePath;

    static FileLogger()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        string relativeLogFilePath = configuration["Logging:LogFilePath"];
        
        string projectRoot = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        _logFilePath = Path.Combine(projectRoot, relativeLogFilePath);
    }

    public static void Log(string message)
    {
        string logMessage = $"{DateTime.Now} - {message}";
        File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
    }
}