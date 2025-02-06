using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.API.AppLogic.Services;
using Portfolio.API.DataAccess;
using Portfolio.API.Domain.ServiceInterfaces;

namespace Portfolio.API.AppLogic
{
    public static class AppLogicSetup
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            DataAccessSetup.AddServices(services, configuration);

            services.AddSingleton<IIpLocationService, IpLocationService>();

            services.AddScoped<ITrackingService, TrackingService>();
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            DataAccessSetup.InitializeDB(serviceProvider);
        }
    }
}
