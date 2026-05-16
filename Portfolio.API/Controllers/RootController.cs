using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.API.Controllers;

[Route("/")]
[ApiController]
public class RootController(IWebHostEnvironment hostingEnv) : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnv = hostingEnv;

    [HttpGet]
    public Ok<string> Index()
    {
        return TypedResults.Ok($"Portfolio API is UP and Running ({_hostingEnv.EnvironmentName})");
    }

    [HttpGet, Route("/admin/host/status")]
    public Ok Status()
    {
        return TypedResults.Ok();
    }
}
