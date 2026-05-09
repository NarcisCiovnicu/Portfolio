using System.Net;

namespace Portfolio.API.Contracts.CustomExceptions;

public class ApiDbException(string message) : ApiException(HttpStatusCode.InternalServerError, message) { }
