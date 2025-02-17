using Microsoft.AspNetCore.Authentication.JwtBearer;
using Portfolio.API.AppLogic;
using Portfolio.API.Domain;
using Portfolio.API.Domain.ConfigOptions;
using Portfolio.API.Logging;
using Portfolio.API.Middlewares;
using Portfolio.API.OptionsSetup;

namespace Portfolio.API
{
    public static class PortfolioApiSetup
    {
        public static void ConfigureLogging(this ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddAzureWebAppDiagnostics();
            logging.AddConsole(opt =>
            {
                opt.FormatterName = Constants.Logging.ConsoleFormatterName;
            });
            logging.AddConsoleFormatter<ApiConsoleFormatter, ApiLogFormatterOptions>();
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptionsWithValidateOnStart<JwtTokenOptions>().BindConfiguration(Constants.Config.JwtToken).ValidateDataAnnotations();
            services.AddOptionsWithValidateOnStart<RateLimitOptions>().BindConfiguration(Constants.Config.RateLimit).ValidateDataAnnotations();
            services.AddOptionsWithValidateOnStart<DatabaseOptions>().Bind(configuration).ValidateDataAnnotations()
                .Validate((options) => options.Validate(), DatabaseOptionsValidation.ErrorMessage);
            services.AddOptionsWithValidateOnStart<CorsConfigOptions>().BindConfiguration(Constants.Config.Cors).ValidateDataAnnotations();

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

            AppLogicSetup.AddServices(services, configuration);
        }
    }
}
