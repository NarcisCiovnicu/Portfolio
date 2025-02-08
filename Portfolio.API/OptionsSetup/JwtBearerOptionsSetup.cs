using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Portfolio.API.Domain.ConfigOptions;
using System.Text;

namespace Portfolio.API.OptionsSetup
{
    public class JwtBearerOptionsSetup(IOptions<JwtTokenOptions> options) : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtTokenOptions _tokenOptions = options.Value;

        public void Configure(string? _, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                //ClockSkew = TimeSpan.Zero, // Has (extra) 5 min by default
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _tokenOptions.Issuer,
                ValidAudience = _tokenOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecretKey))
            };
        }
    }
}
