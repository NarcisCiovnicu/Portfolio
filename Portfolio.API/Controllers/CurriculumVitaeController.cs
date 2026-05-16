using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.Contracts.ServiceInterfaces;

namespace Portfolio.API.Controllers;

[Route("api/cv")]
[ApiController]
public class CurriculumVitaeController(ICVService cvService) : ControllerBase
{
    private readonly ICVService _cvService = cvService;

    [HttpGet]
    public Task<CurriculumVitaeDTO> Get(CancellationToken cancellationToken)
    {
        return _cvService.GetCV(cancellationToken);
    }

    [HttpPost, Authorize]
    public Task<CurriculumVitaeDTO> Update([FromBody] CurriculumVitaeDTO curriculumVitaeDTO, CancellationToken cancellationToken)
    {
        return _cvService.Update(curriculumVitaeDTO, cancellationToken);
    }

    //[HttpGet]
    //public IActionResult Get()
    //{
    //    return Ok(System.IO.File.ReadAllText("./sample-data/cv.json"));
    //}

#if DEBUG
    [HttpPost, Authorize, Route("/api/update-cv-with-mock-data_DebugOnly")]
    public Task<CurriculumVitaeDTO> MockCV(CancellationToken cancellationToken)
    {
        CurriculumVitaeDTO cvDTO = System.Text.Json.JsonSerializer.Deserialize<CurriculumVitaeDTO>(System.IO.File.ReadAllText("./sample-data/cv.json"))!;
        return _cvService.Update(cvDTO, cancellationToken);
    }
#endif
}
