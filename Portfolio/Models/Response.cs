namespace Portfolio.Models
{
    public class Response<T>(T? result, string? errorMessage = null, string? warningMessage = null)
    {
        public T? Result { get; } = result;
        public string? ErrorMessage { get; } = errorMessage;
        public string? WarningMessage { get; } = warningMessage;

        public void Deconstruct(out T? result, out string? errorMessage, out string? warningMessage)
        {
            result = Result;
            errorMessage = ErrorMessage;
            warningMessage = WarningMessage;
        }

        public void Deconstruct(out T? result, out string? errorMessage)
        {
            result = Result;
            errorMessage = ErrorMessage;
        }
    }
}
