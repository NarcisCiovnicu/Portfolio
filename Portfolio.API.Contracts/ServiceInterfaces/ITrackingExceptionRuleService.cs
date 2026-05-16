using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.Contracts.ServiceInterfaces;

public interface ITrackingExceptionRuleService
{
    Task<List<TrackingExceptionRuleDTO>> GetAllRules();
    Task Add(TrackingExceptionRuleDTO ruleDTO);
}
