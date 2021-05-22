using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileLogger.Extensions
{
    public static class RegisterExtensions
    {
        private static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            builder.Services
                .TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());

            //LoggerProviderOptions
            //    .RegisterProviderOptions<FileLoggerSettings, FileLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddOptions<FileLoggerSettings>().Bind(configuration.GetSection("FileLogging"));
            builder.AddFileLogger();
            return builder;
        }
    }
}