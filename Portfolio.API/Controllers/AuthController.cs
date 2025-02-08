using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.ServiceInterfaces;

namespace Portfolio.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationDTO authDto)
        {
            bool isValid = await _authService.IsValid(authDto);
            return isValid ? Ok(_authService.GenerateJwtToken()) : BadRequest("Wrong password");
        }
    }
}
