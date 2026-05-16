using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Contracts.DataTransferObjects;

public record TrackingExceptionRuleDTO(
    [Required]
    [EnumDataType(typeof(TrackingExceptionRuleType))]
    TrackingExceptionRuleType RuleType,
    [Required]
    string Value
);

public enum TrackingExceptionRuleType
{
    Path = 0,
    IpAddress = 1
}
