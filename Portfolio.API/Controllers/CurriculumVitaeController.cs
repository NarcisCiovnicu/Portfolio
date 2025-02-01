using Microsoft.AspNetCore.Mvc;

namespace Portfolio.API.Controllers
{
    [Route("api/cv")]
    [ApiController]
    public class CurriculumVitaeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var json = GetCV();

            return Ok(json);
        }

        // TODO: To be removed
        private static string GetCV()
        {
            return System.IO.File.ReadAllText(@"./sample-data/cv.json");
        }
    }
}
