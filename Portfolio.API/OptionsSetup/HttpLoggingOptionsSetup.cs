using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;

namespace Portfolio.API.OptionsSetup
{
    public class HttpLoggingOptionsSetup : IConfigureOptions<HttpLoggingOptions>
    {
        public void Configure(HttpLoggingOptions options)
        {
            options.LoggingFields = HttpLoggingFields.RequestPath
                    | HttpLoggingFields.RequestQuery
                    | HttpLoggingFields.RequestMethod
                    | HttpLoggingFields.ResponseStatusCode
                    | HttpLoggingFields.RequestBody
                    | HttpLoggingFields.ResponseBody
                    | HttpLoggingFields.Duration;
            options.RequestBodyLogLimit = 4000;
            options.ResponseBodyLogLimit = 4000;
            options.CombineLogs = true;
        }
    }
}
