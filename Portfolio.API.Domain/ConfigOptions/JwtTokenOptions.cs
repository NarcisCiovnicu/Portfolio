using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Domain.ConfigOptions
{
    public class JwtTokenOptions
    {
        [Required]
        public required string Issuer { get; init; }
        [Required]
        public required string Audience { get; init; }
        [Required, MinLength(128)]
        public required string SecretKey { get; init; }
        [Range(1, 24 * 30)]
        public int ExpireAfterH { get; init; }
    }
}
