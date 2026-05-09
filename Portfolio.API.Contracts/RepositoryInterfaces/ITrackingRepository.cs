using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.Contracts.RepositoryInterfaces;

public interface ITrackingRepository
{
    Task Create(ApiTrackerDTO apiTrackerDto);
}
