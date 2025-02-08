using Microsoft.AspNetCore.Http;

namespace Portfolio.API.Domain.CustomExceptions
{
    public class ApiDbException(string message) : ApiException(StatusCodes.Status500InternalServerError, message) { }
}
