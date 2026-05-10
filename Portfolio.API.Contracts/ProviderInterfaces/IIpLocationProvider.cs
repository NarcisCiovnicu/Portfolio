using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.Contracts.ProviderInterfaces;

public interface IIpLocationProvider
{
    Task<IpLocationDTO> GetLocation(string ip);
}
