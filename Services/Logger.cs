using System;
using System.IO;
using TrainingB.Configuration;

namespace TrainingB.Services
{
    public enum LogLevel
    {
        Debug,
        Information,
        Warning,
        Error,
        Critical
    }

    public static class Logger
    {
        private static readonly object _lock = new object();
        private static string? _logFilePath;

        private static string LogFilePath
        {
            get
            {
                if (_logFilePath == null)
                {
                    var config = ConfigurationManager.Config.Logging;
                    string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                    
                    if (!Directory.Exists(logDir))
                    {
                        Directory.CreateDirectory(logDir);
                    }

                    string fileName = $"app-{DateTime.Now:yyyyMMdd}.log";
                    _logFilePath = Path.Combine(logDir, fileName);
                }
                return _logFilePath;
            }
        }

        public static void Log(LogLevel level, string message, Exception? exception = null)
        {
            try
            {
                lock (_lock)
                {
                    string logEntry = FormatLogEntry(level, message, exception);
                    
                    // Write to file
                    File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
                    
                    // Also write to console for debugging
                    Console.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                // Fallback to console only if file logging fails
                Console.WriteLine($"Logging failed: {ex.Message}");
                Console.WriteLine($"Original message: {message}");
            }
        }

        private static string FormatLogEntry(LogLevel level, string message, Exception? exception)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string logMessage = $"[{timestamp}] [{level}] {message}";
            
            if (exception != null)
            {
                logMessage += $"{Environment.NewLine}Exception: {exception.Message}{Environment.NewLine}{exception.StackTrace}";
            }
            
            return logMessage;
        }

        public static void Debug(string message) => Log(LogLevel.Debug, message);
        public static void Info(string message) => Log(LogLevel.Information, message);
        public static void Warning(string message) => Log(LogLevel.Warning, message);
        public static void Error(string message, Exception? exception = null) => Log(LogLevel.Error, message, exception);
        public static void Critical(string message, Exception? exception = null) => Log(LogLevel.Critical, message, exception);

        public static void CleanupOldLogs()
        {
            try
            {
                var config = ConfigurationManager.Config.Logging;
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                
                if (!Directory.Exists(logDir))
                    return;

                var files = Directory.GetFiles(logDir, "app-*.log");
                var cutoffDate = DateTime.Now.AddDays(-config.RetainDays);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        File.Delete(file);
                        Info($"Deleted old log file: {fileInfo.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cleaning up old logs: {ex.Message}");
            }
        }
    }
}
