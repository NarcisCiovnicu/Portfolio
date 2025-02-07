using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Domain.ServiceInterfaces;

namespace Portfolio.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController(IPasswordService passwordService) : ControllerBase
    {
        private readonly IPasswordService _passwordService = passwordService;

        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] string password)
        {
            var result = await _passwordService.HasPassword(password);
            return Ok(result);
        }
    }
}
