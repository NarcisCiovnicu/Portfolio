using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.Domain.RepositoryInterfaces
{
    public interface ITrackingRepository
    {
        Task Create(ApiTrackerDTO apiTrackerDto);
    }
}
