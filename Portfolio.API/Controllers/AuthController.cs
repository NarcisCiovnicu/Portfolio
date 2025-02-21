using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.ServiceInterfaces;
using Portfolio.API.Extensions;

namespace Portfolio.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationDTO authDTO, CancellationToken cancellationToken)
        {
            bool isValid = await _authService.IsValid(authDTO, cancellationToken);

            return isValid
                ? Ok(_authService.GenerateJwtToken())
                : this.UnauthorizedProblem("Wrong password");
        }
    }
}
