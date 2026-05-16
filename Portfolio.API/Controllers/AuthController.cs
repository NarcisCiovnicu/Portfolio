using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.Contracts.ServiceInterfaces;
using Portfolio.API.Extensions;
using System.Net.Mime;

namespace Portfolio.API.Controllers;

[Route("api")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private const string WRONG_PASSWORD = "Wrong password";

    private readonly IAuthService _authService = authService;

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized, MediaTypeNames.Application.ProblemJson)]
    [HttpPost, Route("authenticate")]
    public async Task<Results<Ok<AuthTokenDTO>, ProblemHttpResult>> Authenticate(
        [FromBody] AuthenticationDTO authDTO, CancellationToken cancellationToken)
    {
        bool isValid = await _authService.IsValid(authDTO, cancellationToken);

        return isValid
            ? TypedResults.Ok(_authService.GenerateJwtToken())
            : TypedResults.Problem(this.UnauthorizedProblem(WRONG_PASSWORD));
    }
}
