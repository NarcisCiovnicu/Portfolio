using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using MudBlazor;
using MudBlazor.Services;
using Portfolio.Models;
using Portfolio.Services;

namespace Portfolio
{
    public static class WebClientSetup
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
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
            services.AddOptionsWithValidateOnStart<ClientAppConfig>().BindConfiguration(Constants.Config.ClientAppConfig).ValidateDataAnnotations();

            services.AddScoped(sp =>
            {
                string apiUrl = sp.GetRequiredService<IOptions<ClientAppConfig>>().Value.ApiUrl;
                return new HttpClient { BaseAddress = new Uri(apiUrl) };
            });
            services.AddScoped<ICurriculumVitaeService, CurriculumVitaeService>();
            services.AddScoped<IAppThemeService, AppThemeService>();
        }
    }
}
