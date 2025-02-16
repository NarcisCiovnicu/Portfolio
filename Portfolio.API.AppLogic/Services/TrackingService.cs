using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.RepositoryInterfaces;
using Portfolio.API.Domain.ServiceInterfaces;

namespace Portfolio.API.AppLogic.Services
{
    internal class TrackingService(ILogger<TrackingService> logger, IServiceScopeFactory serviceScopeFactory) : ITrackingService
    {
        private readonly ILogger<TrackingService> _logger = logger;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        public void LogWithFireAndForget(ApiTrackerDTO apiTrackerDTO)
        {
            _ = Task.Run(async () => await Create(apiTrackerDTO));
        }

        private async Task Create(ApiTrackerDTO apiTrackerDTO)
        {
            IServiceScope scope = _serviceScopeFactory.CreateScope();
            ITrackingRepository trackingRepository = scope.ServiceProvider.GetRequiredService<ITrackingRepository>();

            try
            {
                int RetriesCount = 3;
                while (RetriesCount > 0)
                {
                    try
                    {
                        await trackingRepository.Create(apiTrackerDTO);
                        RetriesCount = 0;
                    }
                    catch (SqlException ex)
                    {
                        int attempt = 4 - RetriesCount;
                        _logger.LogWarning(ex, "Attempt #{count} failed to save [apiTracker] in DB.", attempt);
                        RetriesCount--;

                        if (RetriesCount > 0)
                        {
                            // #1 - 30 sec
                            // #2 - 60 sec
                            await Task.Delay(attempt * 30_000);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to save [apiTracker] in DB.");
            }
        }
    }
}
