namespace Portfolio.API.Domain.CustomExceptions
{
    public class ApiException(int statusCodes, string message) : Exception(message)
    {
        public int StatusCode { get; init; } = statusCodes;

        public override string ToString()
        {
            return $"Status: {StatusCode} {base.ToString()}";
        }
    }
}
