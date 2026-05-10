using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.Contracts.ProviderInterfaces;
using Portfolio.API.Contracts.ServiceInterfaces;

namespace Portfolio.API.AppLogic.Services;

internal class IpLocationService(IIpLocationProvider locationProvider) : IIpLocationService
{
    private readonly IIpLocationProvider _locationProvider = locationProvider;

    public Task<IpLocationDTO> GetLocation(string ip)
    {
        return _locationProvider.GetLocation(ip);
    }
}
