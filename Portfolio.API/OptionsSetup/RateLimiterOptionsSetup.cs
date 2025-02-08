using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Portfolio.API.Domain.ConfigOptions;
using System.Net;
using System.Threading.RateLimiting;

namespace Portfolio.API.OptionsSetup
{
    public class RateLimiterOptionsSetup(IOptions<RateLimitOptions> options) : IConfigureOptions<RateLimiterOptions>
    {
        private readonly RateLimitOptions _options = options.Value;

        public void Configure(RateLimiterOptions options)
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
            {
                IPAddress remoteIpAddress = context.Connection.RemoteIpAddress ?? IPAddress.Any;

                if (IPAddress.IsLoopback(remoteIpAddress))
                {
                    return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
                }

                return RateLimitPartition.GetTokenBucketLimiter
                    (remoteIpAddress, _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = _options.TokenLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = _options.QueueLimit,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(_options.ReplenishmentPeriod),
                        TokensPerPeriod = _options.TokensPerPeriod,
                        AutoReplenishment = true
                    });
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        }
    }
}
