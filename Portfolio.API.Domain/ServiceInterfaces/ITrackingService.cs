using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.Domain.ServiceInterfaces
{
    public interface ITrackingService
    {
        void LogWithFireAndForget(ApiTrackerDTO apiTrackerDTO);
    }
}
