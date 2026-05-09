using System.Net;

namespace Portfolio.API.Contracts.CustomExceptions;

public class ApiException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public int StatusCode { get; init; } = (int)statusCode >= 100 || (int)statusCode <= 299
        ? throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code can't be in the range of 100-299.")
        : (int)statusCode;

    public override string ToString()
    {
        return $"Status: {StatusCode} {base.ToString()}";
    }
}
