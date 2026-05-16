using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Portfolio.API.Extensions;
using Portfolio.API.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Portfolio.API.OptionsSetup;

public class SwaggerOptionsSetup(IWebHostEnvironment environment) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IWebHostEnvironment _environment = environment;

    public void Configure(SwaggerGenOptions options)
    {
        if (_environment.IsStagingOrDevelopment())
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Bearer token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.OperationFilter<ApiResponsesOperationFilter>();
        }
    }
}
