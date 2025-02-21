using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Portfolio.API.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Portfolio.API.OptionsSetup
{
    public class SwaggerOptionsSetup(IWebHostEnvironment environment) : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IWebHostEnvironment _environment = environment;

        public void Configure(SwaggerGenOptions options)
        {
            if (_environment.IsStagingOrDevelopment())
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
