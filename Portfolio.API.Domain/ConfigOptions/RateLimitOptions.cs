using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Domain.ConfigOptions
{
    public class RateLimitOptions
    {
        [Required, Range(1, 1024)]
        public int TokenLimit { get; init; }
        [Required, Range(0, 1024)]
        public int QueueLimit { get; init; }
        [Required, Range(1, 120)]
        public int ReplenishmentPeriod { get; init; }
        [Required, Range(1, 1024)]
        public int TokensPerPeriod { get; init; }
    }
}
