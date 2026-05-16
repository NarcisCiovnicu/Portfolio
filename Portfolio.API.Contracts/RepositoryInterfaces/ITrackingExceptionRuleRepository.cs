using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.Contracts.RepositoryInterfaces;

public interface ITrackingExceptionRuleRepository
{
    Task<List<TrackingExceptionRuleDTO>> GetAllRules();
    Task Add(TrackingExceptionRuleDTO ruleDTO);
}
