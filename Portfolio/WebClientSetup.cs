using Blazored.LocalStorage;
using MudBlazor;
using MudBlazor.Services;
using Portfolio.Errors;
using Portfolio.Models;
using Portfolio.Services;

namespace Portfolio
{
    public static class WebClientSetup
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            string apiUrl = configuration.GetValue<string>(Constants.Config.ApiUrl) ?? throw new ConfigMissingException(Constants.Config.ApiUrl);

            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 8000;
                config.SnackbarConfiguration.HideTransitionDuration = 300;
                config.SnackbarConfiguration.ShowTransitionDuration = 100;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });
            services.AddBlazoredLocalStorage();
            services.Configure<ClientAppConfig>(configuration.GetSection(Constants.Config.ClientAppConfig));

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });
            services.AddScoped<ICurriculumVitaeService, CurriculumVitaeService>();
            services.AddScoped<IAppThemeService, AppThemeService>();
        }
    }
}
