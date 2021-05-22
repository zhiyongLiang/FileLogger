using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace FileLogger
{
    public class FileLogger : ILogger
    {
        private readonly FileLoggerSettings settings;
        private readonly static object lockObj = new object();
        private readonly string name;

        public FileLogger(string categoryName, FileLoggerSettings settings)
        {
            name = categoryName;
            this.settings = settings;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return default;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!(state is FileLoggerHelper helper) || !IsEnabled(logLevel))
            {
                return;
            }

            try
            {
                Monitor.Enter(lockObj);
                var file = CreateLogFile(helper.CustomSubPath, helper.CustomFileName);
                //如果未能创建日志文件就不记录日志
                if (string.IsNullOrEmpty(file))
                {
                    return;
                }
                var sb = new StringBuilder();
                sb.AppendLine($"\"{name}\"log:");
                sb.Append(formatter(state, exception));
                File.AppendAllText(file, sb.ToString());
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
        }

        private string CreateLogFile(string customSubPath, string customFileName)
        {
            var path = $"{BuildRootPath()}{BuildSubPath(customSubPath)}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var file = $"{path}{ BuildFileName(customFileName)}";
            file = CheckLogFileMaxSizeOrCreateNew(file);
            return file;
        }

        private string CheckLogFileMaxSizeOrCreateNew(string file)
        {
            var maxSize = 5L;
            if (settings.MaxSize <= 50 && settings.MaxSize > 0)
            {
                maxSize = settings.MaxSize * 1024; //KB
                maxSize *= 1024;  //B
            }
            else
            {
                maxSize *= 1024; //KB
                maxSize *= 1024; //B
            }
            var sn = 0;
            while (true)
            {
                var fileInfo = new FileInfo(file);
                if (!fileInfo.Exists)
                {
                    File.Create(file).Close();
                    break;
                }
                if (fileInfo?.Length < maxSize)
                {
                    break;
                }
                file += $".{sn++}";
            }
            return file;
        }

        private string BuildRootPath()
        {
            return string.IsNullOrEmpty(settings.DefaultPath) ? "Log" : settings.DefaultPath;
        }

        private string BuildSubPath(string customSubPath)
        {
            if (!string.IsNullOrEmpty(customSubPath))
            {
                return ($"/{customSubPath}/");
            }
            var path = new StringBuilder();
            Enum.TryParse(settings.SubPath, out SubPath subPath);
            switch (subPath)
            {
                case SubPath.Day:
                    path.Append($"/{DateTime.Now:yyyyMMdd}");
                    break;

                case SubPath.Week:
                    var calendar = new GregorianCalendar();
                    int weekOfYear = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    path.Append($"/{DateTime.Now.Year}{weekOfYear.ToString().PadLeft(2, '0')}W");
                    break;

                case SubPath.Month:
                    path.Append($"/{DateTime.Now:yyyyMM}M");
                    break;
            }
            return path.Append("/").ToString();
        }

        private string BuildFileName(string customFileName)
        {
            if (!string.IsNullOrEmpty(customFileName))
            {
                return customFileName;
            }
            if (!string.IsNullOrEmpty(settings.DefaultFileName))
            {
                return settings.DefaultFileName;
            }
            return $"{DateTime.Now:yyyyMMdd}.log";
        }
    }
}