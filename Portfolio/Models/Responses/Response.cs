namespace Portfolio.Models.Responses
{
    public class Response<R, ProblemType>(R? result, ProblemType? error = null) where ProblemType : ProblemDetails
    {
        public R? Result { get; } = result;
        public ProblemType? Error { get; } = error;

        public bool IsSuccessful { get { return Error is null; } }

        public void Deconstruct(out R? result, out ProblemDetails? error)
        {
            result = Result;
            error = Error;
        }
    }
}
