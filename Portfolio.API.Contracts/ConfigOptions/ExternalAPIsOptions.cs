using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Contracts.ConfigOptions;

public class ExternalAPIsOptions
{
    [Required]
    [ValidateObjectMembers]
    public required IpLocationAPIOptions IpLocationAPI { get; init; }
}

public class IpLocationAPIOptions
{
    [Required, Url]
    public required string BaseUrl { get; init; }
}
