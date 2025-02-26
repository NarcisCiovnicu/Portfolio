using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using MudBlazor;
using MudBlazor.Services;
using Portfolio.Handlers;
using Portfolio.Models;
using Portfolio.Providers;
using Portfolio.Services;

namespace Portfolio
{
    public static class WebClientSetup
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorizationCore();
            services.AddCascadingAuthenticationState();
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

            services.AddScoped<ClientAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ClientAuthStateProvider>());
            services.AddScoped<AuthorizationHandler>();
            services.AddScoped(sp =>
            {
                string apiUrl = sp.GetRequiredService<IOptions<ClientAppConfig>>().Value.ApiUrl;
                AuthorizationHandler handler = sp.GetRequiredService<AuthorizationHandler>();
                return new HttpClient(handler)
                {
                    BaseAddress = new Uri(apiUrl),
                    Timeout = TimeSpan.FromSeconds(Constants.Request.DefaultTimeoutSeconds)
                };
            });
            services.AddScoped<IAuthTokenService, AuthTokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICurriculumVitaeService, CurriculumVitaeService>();
            services.AddScoped<IAppThemeService, AppThemeService>();
        }
    }
}
