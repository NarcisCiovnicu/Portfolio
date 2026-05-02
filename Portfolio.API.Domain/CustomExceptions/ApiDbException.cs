using System.Net;

namespace Portfolio.API.Domain.CustomExceptions
{
    public class ApiDbException(string message) : ApiException(HttpStatusCode.InternalServerError, message) { }
}
