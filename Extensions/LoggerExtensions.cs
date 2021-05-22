using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileLogger.Extensions
{
    public static class LoggerExtensions
    {
        public static void FileLog(this ILogger logger, string message, Exception ex = null, Action<FileLoggerHelper> helperAction = null)
        {
            logger.FileLog(LogLevel.None, 0, message, ex, helperAction);
        }

        public static void FileLogError(this ILogger logger, string message, Exception ex = null, Action<FileLoggerHelper> helperAction = null)
        {
            logger.FileLog(LogLevel.Error, 0, message, ex, helperAction);
        }
         
        public static void FileLogInformation(this ILogger logger, string message, Exception ex = null, Action<FileLoggerHelper> helperAction = null)
        {
            logger.FileLog(LogLevel.Information, 0, message, ex, helperAction);
        }   

        public static void FileLogDebug(this ILogger logger, string message, Exception ex = null, Action<FileLoggerHelper> helperAction = null)
        {
            logger.FileLog(LogLevel.Debug, 0, message, ex, helperAction);
        }  
        
        public static void FileLogWarning(this ILogger logger, string message, Exception ex = null, Action<FileLoggerHelper> helperAction = null)
        {
            logger.FileLog(LogLevel.Warning, 0, message, ex, helperAction);
        }
         
        private static void FileLog(this ILogger logger, LogLevel logLevel, EventId eventId, string message, Exception ex = null, Action<FileLoggerHelper> helperAction = null)
        {
            if (string.IsNullOrEmpty(message) && ex == null)
            {
                return;
            }
            var helper = new FileLoggerHelper();
            helperAction?.Invoke(helper);
            logger.Log(logLevel, eventId, helper, ex, (state, ex) =>
            {
                var sb = new StringBuilder();
                sb.AppendLine(message);
                if (ex != null)
                {
                    sb.AppendLine($"exception: {ex.Message}");
                }
                //加上默认时间
                if (state.IsLogTime)
                {
                    sb.AppendLine($"{DateTime.Now}");
                }
                return sb.ToString();
            });
        }
    }
}