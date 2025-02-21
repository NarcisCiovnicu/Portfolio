using Microsoft.AspNetCore.Http.Extensions;

namespace Portfolio.API.Extensions
{
    public static class HttpRequestExtension
    {
        public static string GetInstance(this HttpRequest request)
        {
            return $"{request.Method} {request.GetEncodedPathAndQuery()}";
        }
    }
}
