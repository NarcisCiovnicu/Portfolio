using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Contracts.ConfigOptions;

public class CorsConfigOptions
{
    [Required, Url]
    public required string WebClientOrigin { get; init; }
}
