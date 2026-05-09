using Microsoft.AspNetCore.Authentication.JwtBearer;
using Portfolio.API.AppLogic;
using Portfolio.API.Domain.ConfigOptions;
using Portfolio.API.Domain.Constants;
using Portfolio.API.Logging;
using Portfolio.API.Middlewares;
using Portfolio.API.OptionsSetup;

namespace Portfolio.API;

public static class PortfolioApiSetup
{
    public static void ConfigureLogging(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddAzureWebAppDiagnostics();
        logging.AddConsole(opt =>
        {
            opt.FormatterName = ConstLogFormaterNames.ApiConsoleFormatter;
        });
        logging.AddConsoleFormatter<ApiConsoleFormatter, ApiLogFormatterOptions>();
    }

    public static void AddServices(IServiceCollection services)
    {
        services.AddOptionsWithValidateOnStart<JwtTokenOptions>().BindConfiguration(ConstConfigSections.JwtToken).ValidateDataAnnotations();
        services.AddOptionsWithValidateOnStart<RateLimitOptions>().BindConfiguration(ConstConfigSections.RateLimit).ValidateDataAnnotations();
        services.AddOptionsWithValidateOnStart<DatabaseOptions>().BindConfiguration(ConstConfigSections.Root).ValidateDataAnnotations()
            .Validate((options) => options.Validate(), DatabaseOptionsValidation.ErrorMessage);
        services.AddOptionsWithValidateOnStart<CorsConfigOptions>().BindConfiguration(ConstConfigSections.Cors).ValidateDataAnnotations();
        services.AddOptionsWithValidateOnStart<ExternalAPIsOptions>().BindConfiguration(ConstConfigSections.ExternalAPIs).ValidateDataAnnotations();

        services.ConfigureOptions<RateLimiterOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.ConfigureOptions<HttpLoggingOptionsSetup>();
        services.ConfigureOptions<CorsOptionsSetup>();
        services.ConfigureOptions<SwaggerOptionsSetup>();

        services.AddControllers();
        services.AddHttpLogging(opt => { });
        services.AddHttpLoggingInterceptor<HttpLoggingInterceptor>();
        services.AddRateLimiter(opt => { });
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.AddCors();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        AppLogicSetup.AddServices(services);
    }
}
