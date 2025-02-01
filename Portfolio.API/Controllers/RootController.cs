using Microsoft.AspNetCore.Mvc;

namespace Portfolio.API.Controllers
{
    [Route("/")]
    [ApiController]
    public class RootController(IWebHostEnvironment hostingEnv) : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnv = hostingEnv;

        [HttpGet]
        public IActionResult Index()
        {
            return Ok($"Portfolio API is UP and Running ({_hostingEnv.EnvironmentName})");
        }
    }
}
