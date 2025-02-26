using Portfolio.Extensions;
using Portfolio.Models.Responses;
using Shared;
using System.Net.Http.Json;
using System.Text.Json;

namespace Portfolio.Services
{
    // TODO: move to an external library
    public abstract class BaseApiService<ProblemType>(ILogger logger, HttpClient httpClient) where ProblemType : ProblemDetails, new()
    {
        private readonly ILogger _logger = logger;
        private readonly HttpClient _httpClient = httpClient;

        protected Task<Response<ResultType, ProblemType>> HttpGetAsync<ResultType>(string url, CancellationToken cancellationToken = default)
        {
            return SendHttpRequest<ResultType>(() => _httpClient.GetAsync(url, cancellationToken));
        }

        protected Task<Response<ResultType, ProblemType>> HttpPostAsync<T, ResultType>(string url, T value, CancellationToken cancellationToken = default)
        {
            return SendHttpRequest<ResultType>(() => _httpClient.PostAsJsonAsync(url, value, cancellationToken));
        }

        protected virtual ProblemType CreateProblemDetails(HttpResponseMessage failedResponse)
        {
            int statusCode = (int)failedResponse.StatusCode;
            return new ProblemType()
            {
                Status = statusCode,
                Title = "Sorry, something went wrong.",
                Detail = failedResponse.ReasonPhrase ?? string.Empty,
                Type = ProblemDetailsHelper.GetProblemDetailsType(statusCode),
                Instance = failedResponse.RequestMessage?.GetRequestInstance(),
            };
        }

        protected virtual ProblemType CreateProblemDetails(Exception exception)
        {
            return new ProblemType()
            {
                Title = "Sorry, something went wrong.",
                Detail = $"Connection problem ({exception.Message})",
            };
        }

        protected virtual ProblemType CreateProblemDetails(OperationCanceledException exception)
        {
            return new ProblemType()
            {
                Title = "Request was cancelled.",
            };
        }

        protected virtual ProblemType CreateProblemDetails(TimeoutException exception)
        {
            return new ProblemType()
            {
                Title = "Request timeout.",
                Detail = "The request took too long to complete. Try again later."
            };
        }

        protected virtual ProblemType CreateProblemDetails(JsonException exception)
        {
            return new ProblemType()
            {
                Title = "Unexpected error.",
                Detail = "Failed to parse JSON response."
            };
        }

        private async Task<Response<ResultType, ProblemType>> SendHttpRequest<ResultType>(Func<Task<HttpResponseMessage>> requestFunction)
        {
            try
            {
                HttpResponseMessage response = await requestFunction().ConfigureAwait(false);

                return await CreateResponseAsync<ResultType>(response).ConfigureAwait(false);
            }
            catch (TimeoutException ex)
            {
                _logger.LogWarning("API request timed out. {message}", ex.Message);
                return new Response<ResultType, ProblemType>(default, CreateProblemDetails(ex));
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning("API request was cancelled. {message}", ex.Message);
                if (ex.InnerException is TimeoutException timeoutException)
                {
                    return new Response<ResultType, ProblemType>(default, CreateProblemDetails(timeoutException));
                }
                return new Response<ResultType, ProblemType>(default, CreateProblemDetails(ex));
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON response.");
                return new Response<ResultType, ProblemType>(default, CreateProblemDetails(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API request failed.");
                return new Response<ResultType, ProblemType>(default, CreateProblemDetails(ex));
            }
        }

        private async Task<Response<ResultType, ProblemType>> CreateResponseAsync<ResultType>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await GetResultAsync<ResultType>(response).ConfigureAwait(false);
            }
            else
            {
                return await GetErrorAsync<ResultType>(response).ConfigureAwait(false);
            }
        }

        private static async Task<Response<R, ProblemType>> GetResultAsync<R>(HttpResponseMessage response)
        {
            R? result = await response.Content.ReadFromJsonAsync<R>().ConfigureAwait(false);
            return new Response<R, ProblemType>(result);
        }

        private async Task<Response<R, ProblemType>> GetErrorAsync<R>(HttpResponseMessage failedResponse)
        {
            ProblemType? problemDetails = await TryParseAsProblemDetails(failedResponse).ConfigureAwait(false);
            problemDetails ??= CreateProblemDetails(failedResponse);
            return new Response<R, ProblemType>(default, problemDetails);
        }

        private static async ValueTask<ProblemType?> TryParseAsProblemDetails(HttpResponseMessage response)
        {
            if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
            {
                return await response.Content.ReadFromJsonAsync<ProblemType>().ConfigureAwait(false);
            }
            return null;
        }
    }
}
