using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.Contracts.RepositoryInterfaces;
using Portfolio.API.Contracts.ServiceInterfaces;

namespace Portfolio.API.AppLogic.Services;

internal class TrackingExceptionRuleService(ITrackingExceptionRuleRepository ruleRepository) : ITrackingExceptionRuleService
{
    private readonly ITrackingExceptionRuleRepository _ruleRepository = ruleRepository;

    public Task<List<TrackingExceptionRuleDTO>> GetAllRules()
    {
        return _ruleRepository.GetAllRules();
    }

    public Task Add(TrackingExceptionRuleDTO ruleDTO)
    {
        return _ruleRepository.Add(ruleDTO);
    }
}
