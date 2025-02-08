using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.API.AppLogic.Services;
using Portfolio.API.DataAccess;
using Portfolio.API.Domain;
using Portfolio.API.Domain.ServiceInterfaces;

namespace Portfolio.API.AppLogic
{
    public static class AppLogicSetup
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            DataAccessSetup.AddServices(services, configuration);

            services.AddHttpClient(Constants.IpLocationApi.Name, config =>
            {
                config.BaseAddress = new Uri(Constants.IpLocationApi.BaseUrl);
                config.Timeout = TimeSpan.FromSeconds(10);
            });

            services.AddSingleton<IIpLocationService, IpLocationService>();

            services.AddScoped<ITrackingService, TrackingService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            DataAccessSetup.InitializeDB(serviceProvider);
        }
    }
}
