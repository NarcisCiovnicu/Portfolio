using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Portfolio.API.OptionsSetup
{
    public class SwaggerOptionsSetup(IWebHostEnvironment hostEnvironment) : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IWebHostEnvironment _hostEnvironment = hostEnvironment;

        public void Configure(SwaggerGenOptions options)
        {
            if (_hostEnvironment.IsDevelopment() || _hostEnvironment.IsStaging())
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Bearer token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference()
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            }
        }
    }
}
