using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileLogger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly FileLoggerSettings settings;

        public FileLoggerProvider(IOptionsMonitor<FileLoggerSettings> fileLoggerSettingsOptions)
        {
            settings = fileLoggerSettingsOptions.CurrentValue;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, settings);
        }

        public void Dispose()
        {
        }
    }
}