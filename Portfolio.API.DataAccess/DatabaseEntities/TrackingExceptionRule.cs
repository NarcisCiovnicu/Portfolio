using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.DataAccess.DatabaseEntities;

public class TrackingExceptionRule : BaseEntity
{
    public required TrackingExceptionRuleType RuleType { get; set; }

    public required string Value { get; set; }
}
