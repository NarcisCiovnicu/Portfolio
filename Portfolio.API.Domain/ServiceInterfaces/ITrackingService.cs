using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.Domain.ServiceInterfaces
{
    public interface ITrackingService
    {
        Task Create(ApiTrackerDTO trackerDto);
    }
}
