using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Portfolio.API.AppLogic.Services;
using Portfolio.API.DataAccess;
using Portfolio.API.Domain.ConfigOptions;
using Portfolio.API.Domain.Constants;
using Portfolio.API.Domain.ServiceInterfaces;

namespace Portfolio.API.AppLogic;

public static class AppLogicSetup
{
    public static void AddServices(IServiceCollection services)
    {
        DataAccessSetup.AddServices(services);

        services.AddHttpClient(ConstHttpClientNames.IpLocationApi, (sp, httpClient) =>
        {
            IpLocationAPIOptions options = sp.GetRequiredService<IOptions<ExternalAPIsOptions>>().Value.IpLocationAPI;
            httpClient.BaseAddress = new Uri(options.BaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(10);
        });

        services.AddSingleton<IIpLocationService, IpLocationService>();
        services.AddSingleton<ITrackingService, TrackingService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICVService, CVService>();
    }

    public static void Initialize(IServiceProvider serviceProvider)
    {
        DataAccessSetup.InitializeDB(serviceProvider);
    }
}
