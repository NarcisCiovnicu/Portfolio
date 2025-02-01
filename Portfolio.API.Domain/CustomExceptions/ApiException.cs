namespace Portfolio.API.Domain.CustomExceptions
{
    public class ApiException(string message) : Exception(message) { }
}
