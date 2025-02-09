using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Portfolio.API.Domain;

namespace Portfolio.API.Logging
{
    public class ApiConsoleFormatter(IOptions<ApiLogFormatterOptions> options) : ConsoleFormatter(Constants.Logging.ConsoleFormatterName)
    {
        private readonly ApiLogFormatterOptions _options = options.Value;

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
        {
            string category = logEntry.Category;
            string eventId = logEntry.EventId.Id.ToString();

            if (_options.UseAzureFormat)
            {
                string logLevel = GetAzureLogLevel(logEntry.LogLevel);
                textWriter.WriteLine($"{logLevel}\t{category}[{eventId}]");
                textWriter.WriteLine($"{logLevel}\t{logEntry.State?.ToString()?.ReplaceLineEndings(" ")}");
                if (logEntry.Exception is not null)
                {
                    textWriter.WriteLine($"{logLevel}\t{logEntry.Exception.ToString().ReplaceLineEndings(" ")}");
                }
            }
            else
            {
                string logLevel = GetLogLevel(logEntry.LogLevel);
                textWriter.WriteLine($"{logLevel}\t{category}[{eventId}]");
                textWriter.WriteLine($"\t{logEntry.State?.ToString()?.ReplaceLineEndings($"{Environment.NewLine}\t")}");
                if (logEntry.Exception is not null)
                {
                    textWriter.WriteLine($"\t{logEntry.Exception.ToString().ReplaceLineEndings($"{Environment.NewLine}\t")}");
                }
                textWriter.WriteLine();
            }
        }

        private string GetLogLevel(LogLevel logLevel)
        {
            (string logLevelName, string format) = logLevel switch
            {
                LogLevel.Trace => ("TRACE", "\x1B[1m"),
                LogLevel.Debug => ("DEBUG", "\x1B[1m\x1B[34m"),
                LogLevel.Information => ("INFO", "\x1B[1m\x1B[32m"),
                LogLevel.Warning => ("WARN", "\x1B[1m\x1B[33m"),
                LogLevel.Error => ("ERROR", "\x1B[1m\x1B[31m"),
                LogLevel.Critical => ("CRIT", "\x1B[1m\x1B[41m"),
                _ => ("", "")
            };

            if (_options.ColorBehavior == LoggerColorBehavior.Enabled)
            {
                return $"{format}{logLevelName}\x1B[0m";
            }

            return logLevelName;
        }

        private static string GetAzureLogLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => "TRACE",
                LogLevel.Debug => "DEBUG",
                LogLevel.Information => "INFO",
                LogLevel.Warning => "WARNING",
                LogLevel.Error => "ERROR",
                LogLevel.Critical => "[ERROR]",
                _ => ""
            };
        }
    }
}
