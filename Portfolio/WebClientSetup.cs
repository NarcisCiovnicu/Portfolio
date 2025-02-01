using Blazored.LocalStorage;
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
            string apiUrl = configuration.GetValue<string>(Constants.ApiUrl) ?? throw new ConfigMissingException(Constants.ApiUrl);

            services.AddMudServices();
            services.AddBlazoredLocalStorage();
            services.Configure<ClientAppConfig>(configuration.GetSection(Constants.ClientAppConfig));

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });
            services.AddScoped<ICurriculumVitaeService, CurriculumVitaeService>();
        }
    }
}
