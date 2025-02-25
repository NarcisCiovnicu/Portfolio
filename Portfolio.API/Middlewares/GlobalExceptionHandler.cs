using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Domain.CustomExceptions;
using Portfolio.API.Extensions;
using Shared;
using System.Text.Json;

namespace Portfolio.API.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment environment) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;
        private readonly IWebHostEnvironment _environment = environment;

        private static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

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
            await httpContext.Response.WriteAsJsonAsync(problem, _serializerOptions, contentType: "application/problem+json", cancellationToken);

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
            problem.Type = ProblemDetailsHelper.GetProblemDetailsType(problem.Status ?? 0);
            problem.Instance = httpContext.Request.GetInstance();
            problem.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if (_environment.IsStagingOrDevelopment())
            {
                problem.Extensions.Add("exception", ex.ToString());
            }
        }
    }
}
