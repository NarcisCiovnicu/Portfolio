using Common.Utils;
using Microsoft.AspNetCore.HttpLogging;
using Portfolio.API.Domain;
using System.Net;
using System.Threading.RateLimiting;

namespace Portfolio.API
{
    public static class PortfolioApiSetup
    {
        public static void SetupWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestPath
                    | HttpLoggingFields.RequestQuery
                    | HttpLoggingFields.RequestMethod
                    | HttpLoggingFields.ResponseStatusCode
                    | HttpLoggingFields.RequestBody
                    | HttpLoggingFields.ResponseBody
                    | HttpLoggingFields.Duration;
                options.RequestBodyLogLimit = 4096;
                options.ResponseBodyLogLimit = 4096;
                options.CombineLogs = true;
            });

            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
                {
                    IPAddress remoteIpAddress = context.Connection.RemoteIpAddress ?? IPAddress.Any;

                    if (IPAddress.IsLoopback(remoteIpAddress))
                    {
                        return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
                    }

                    int tokenLimit = configuration.TryGetTypeValue<int>(Constants.Config.RateLimit.BucketTokenLimit);
                    int queueLimit = configuration.TryGetTypeValue<int>(Constants.Config.RateLimit.BucketQueueLimit);
                    int replenishmentPeriod = configuration.TryGetTypeValue<int>(Constants.Config.RateLimit.BucketReplenishmentPeriod);
                    int tokensPerPeriod = configuration.TryGetTypeValue<int>(Constants.Config.RateLimit.BucketTokensPerPeriod);

                    return RateLimitPartition.GetTokenBucketLimiter
                        (remoteIpAddress, _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = tokenLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = queueLimit,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(replenishmentPeriod),
                            TokensPerPeriod = tokensPerPeriod,
                            AutoReplenishment = true
                        });
                });

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
        }

        public static void AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            string webClientOrigin = configuration.TryGetValue<string>(Constants.Config.WebClientOrigin);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    policyBuilder.AllowAnyHeader().AllowAnyMethod();
                    policyBuilder.WithOrigins(webClientOrigin);
                });
            });
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //
        }
    }
}
