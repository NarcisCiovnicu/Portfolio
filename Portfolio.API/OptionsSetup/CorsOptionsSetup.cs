using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using Portfolio.API.Domain.ConfigOptions;

namespace Portfolio.API.OptionsSetup
{
    public class CorsOptionsSetup(IOptions<CorsConfigOptions> options) : IConfigureOptions<CorsOptions>
    {
        private readonly CorsConfigOptions _options = options.Value;

        public void Configure(CorsOptions options)
        {
            options.AddDefaultPolicy(policyBuilder =>
            {
                policyBuilder.AllowAnyHeader().AllowAnyMethod();
                policyBuilder.WithOrigins(_options.WebClientOrigin);
            });
        }
    }
}
