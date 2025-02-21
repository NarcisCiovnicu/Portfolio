using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Domain;
using Portfolio.API.Domain.CustomExceptions;
using Portfolio.API.Extensions;

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
                DbUpdateException dbException => UnexpectedException("Database Update Error"),
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

        private static ProblemDetails UnexpectedException(string? title = null)
        {
            return new ProblemDetails()
            {
                Title = title ?? "Internal Server Error",
                Detail = "There was an unexpected error processing your request. Please try again. If error persist contact me.",
                Status = StatusCodes.Status500InternalServerError
            };
        }

        private void AddMoreDetails(ProblemDetails problem, Exception ex, HttpContext httpContext)
        {
            problem.Type = GetProblemType(problem.Status ?? 0);
            problem.Instance = httpContext.Request.GetInstance();
            problem.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if (_environment.IsStagingOrDevelopment())
            {
                problem.Extensions.Add("exception", ex.ToString());
            }
        }

        private static string GetProblemType(int statusCode)
        {
            if (statusCode < 100)
            {
                return Constants.ProblemDetailsType.Default;
            }
            else if (statusCode < 200)
            {
                return Constants.ProblemDetailsType.Status100;
            }
            else if (statusCode < 300)
            {
                return Constants.ProblemDetailsType.Status200;
            }
            else if (statusCode < 400)
            {
                return Constants.ProblemDetailsType.Status300;
            }
            else if (statusCode < 500)
            {
                return Constants.ProblemDetailsType.Status400;
            }
            else if (statusCode < 600)
            {
                return Constants.ProblemDetailsType.Status500;
            }
            return Constants.ProblemDetailsType.Default;
        }
    }
}
