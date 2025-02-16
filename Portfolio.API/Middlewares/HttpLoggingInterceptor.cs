using Microsoft.AspNetCore.HttpLogging;

namespace Portfolio.API.Middlewares
{
    public class HttpLoggingInterceptor : IHttpLoggingInterceptor
    {
        public ValueTask OnRequestAsync(HttpLoggingInterceptorContext logContext)
        {
            var path = logContext.HttpContext.Request.Path.Value ?? "";
            if (path.Contains("/api/authenticate"))
            {
                logContext.Disable(HttpLoggingFields.RequestBody);
            }
            return default;
        }

        public ValueTask OnResponseAsync(HttpLoggingInterceptorContext logContext)
        {
            return default;
        }
    }
}
