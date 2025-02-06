using Microsoft.Extensions.Logging;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.RepositoryInterfaces;
using Portfolio.API.Domain.ServiceInterfaces;

namespace Portfolio.API.AppLogic.Services
{
    internal class TrackingService(ILogger<TrackingService> logger, ITrackingRepository trackingRepository) : ITrackingService
    {
        private readonly ILogger<TrackingService> _logger = logger;
        private readonly ITrackingRepository _trackingRepository = trackingRepository;

        public async Task Create(ApiTrackerDTO trackerDto)
        {
            try
            {
                await _trackingRepository.Create(trackerDto).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save [apiTracker] in DB");
            }
        }
    }
}
