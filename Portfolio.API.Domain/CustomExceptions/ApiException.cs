namespace Portfolio.API.Domain.CustomExceptions
{
    public class ApiException(int statusCode, string message) : Exception(message)
    {
        public int StatusCode { get; init; } = statusCode >= 200 || statusCode <= 299
            ? throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code can't be in the range of 200-299.")
            : statusCode;

        public override string ToString()
        {
            return $"Status: {StatusCode} {base.ToString()}";
        }
    }
}
