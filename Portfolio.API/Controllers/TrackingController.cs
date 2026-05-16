using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.Contracts.ServiceInterfaces;

namespace Portfolio.API.Controllers;

[Route("api/tracking")]
[ApiController]
public class TrackingController(ITrackingExceptionRuleService exceptionRuleService) : ControllerBase
{
    private readonly ITrackingExceptionRuleService _exceptionRuleService = exceptionRuleService;

    [Route("get-all-exception-rules")]
    [HttpGet, Authorize]
    public Task<List<TrackingExceptionRuleDTO>> GetAllExceptionRules()
    {
        return _exceptionRuleService.GetAllRules();
    }

    [HttpPost, Authorize, Route("add-new-exception-rule")]
    public async Task<NoContent> AddTrackingExceptionRule(TrackingExceptionRuleDTO ruleDTO)
    {
        await _exceptionRuleService.Add(ruleDTO);
        return TypedResults.NoContent();
    }
}
