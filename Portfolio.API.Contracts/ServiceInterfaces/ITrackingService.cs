using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.Contracts.ServiceInterfaces;

public interface ITrackingService
{
    /// <summary>
    /// Don't wait for the Task, but it's returned for unit tests
    /// </summary>
    Task LogWithFireAndForget(ApiTrackerDTO apiTrackerDTO);
}
