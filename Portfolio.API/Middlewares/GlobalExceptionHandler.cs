using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Domain.CustomExceptions;

namespace Portfolio.API.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment environment) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;
        private readonly IWebHostEnvironment _environment = environment;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ApiException)
            {
                _logger.LogError(exception, "[API Exception]");
            }
            else
            {
                _logger.LogCritical(exception, "[{name}]", nameof(GlobalExceptionHandler));
            }

            ProblemDetails problem = exception switch
            {
                ApiException apiException => ApiException(apiException),
                Exception _ => UnexpectedException()
            };

            AddMoreDetails(problem, exception, httpContext);

            httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }

        private static ProblemDetails ApiException(ApiException ex)
        {
            return new ProblemDetails()
            {
                Title = "API Error",
                Detail = ex.Message,
                Status = ex.StatusCode
            };
        }

        private static ProblemDetails UnexpectedException()
        {
            return new ProblemDetails()
            {
                Title = "Internal Server Error",
                Detail = "There was an unexpected error processing your request. Please try again. If error persist contact me.",
                Status = StatusCodes.Status500InternalServerError
            };
        }

        private void AddMoreDetails(ProblemDetails problem, Exception ex, HttpContext httpContext)
        {
            problem.Type = GetProblemType(problem.Status ?? 0);
            problem.Instance = $"{httpContext.Request.Method} {httpContext.Request.GetEncodedPathAndQuery()}";
            problem.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if (_environment.IsDevelopment())
            {
                problem.Extensions.Add("stackTrace", ex.StackTrace);
            }
        }

        private static string GetProblemType(int statusCode)
        {
            if (statusCode < 100)
            {
                return "about:blank";
            }
            else if (statusCode < 200)
            {
                return "https://datatracker.ietf.org/doc/html/rfc9110#section-15.2";
            }
            else if (statusCode < 300)
            {
                return "https://datatracker.ietf.org/doc/html/rfc9110#section-15.3";
            }
            else if (statusCode < 400)
            {
                return "https://datatracker.ietf.org/doc/html/rfc9110#section-15.4";
            }
            else if (statusCode < 500)
            {
                return "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5";
            }
            else if (statusCode < 600)
            {
                return "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6";
            }
            return "about:blank";
        }
    }
}
