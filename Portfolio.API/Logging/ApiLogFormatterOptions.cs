using Microsoft.Extensions.Logging.Console;

namespace Portfolio.API.Logging
{
    public class ApiLogFormatterOptions : SimpleConsoleFormatterOptions
    {
        public bool UseAzureFormat { get; set; }

        public ApiLogFormatterOptions()
        {
            ColorBehavior = Console.IsOutputRedirected ? LoggerColorBehavior.Disabled : LoggerColorBehavior.Enabled;
        }
    }
}
