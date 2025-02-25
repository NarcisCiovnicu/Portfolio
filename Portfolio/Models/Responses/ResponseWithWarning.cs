namespace Portfolio.Models.Responses
{
    public class ResponseWithWarning<T> : Response<T, ProblemDetails>
    {
        public string? WarningMessage { get; } = null;

        public ResponseWithWarning(Response<T, ProblemDetails> response, string? warningMessage = null) : base(response.Result, response.Error)
        {
            WarningMessage = warningMessage;
        }

        public ResponseWithWarning(T? result, ProblemDetails? error = null, string? warningMessage = null) : base(result, error)
        {
            WarningMessage = warningMessage;
        }

        public void Deconstruct(out T? result, out ProblemDetails? error, out string? warningMessage)
        {
            result = Result;
            error = Error;
            warningMessage = WarningMessage is null ? null : $"😧 {WarningMessage}";
        }
    }
}
