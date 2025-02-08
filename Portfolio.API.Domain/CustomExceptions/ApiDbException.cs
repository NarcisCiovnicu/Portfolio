namespace Portfolio.API.Domain.CustomExceptions
{
    public class ApiDbException(string message) : ApiException(message) { }
}
