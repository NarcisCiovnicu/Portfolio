using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.Contracts.ServiceInterfaces;

public interface IIpLocationService
{
    Task<IpLocationResponseDTO> GetLocation(string ip);
}
