using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.Domain.ServiceInterfaces
{
    public interface IIpLocationService
    {
        Task<IpLocationResponseDTO> GetLocation(string ip);
    }
}
