using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.ServiceInterfaces;

namespace Portfolio.API.Controllers
{
    [Route("api/cv")]
    [ApiController]
    public class CurriculumVitaeController(ICVService cvService) : ControllerBase
    {
        private readonly ICVService _cvService = cvService;

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            CurriculumVitaeDTO cv = await _cvService.GetCV(cancellationToken);
            return Ok(cv);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CurriculumVitaeDTO curriculumVitaeDTO, CancellationToken cancellationToken)
        {
            CurriculumVitaeDTO updatedCV = await _cvService.Update(curriculumVitaeDTO, cancellationToken);
            return Ok(updatedCV);
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok(ReadSampleCV());
        //}

        //// For testing without DB
        //private static string ReadSampleCV()
        //{
        //    return System.IO.File.ReadAllText(@"./sample-data/cv.json");
        //}
    }
}
