using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.RepositoryInterfaces;
using Portfolio.API.Domain.ServiceInterfaces;

namespace Portfolio.API.AppLogic.Services
{
    /// <param name="serviceScopeFactory">To create scoped repository</param>
    /// <param name="retryDelay"> Used only for easy unit testing. You can leave default value otherwise.</param>
    internal class TrackingService(ILogger<TrackingService> logger, IServiceScopeFactory serviceScopeFactory, int retryDelay = 30_000) : ITrackingService
    {
        private readonly ILogger<TrackingService> _logger = logger;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private const int _maxAttempts = 3;

        internal int RetryDelay { get; init; } = Math.Clamp(retryDelay, 1, 60_000);

        public Task LogWithFireAndForget(ApiTrackerDTO apiTrackerDTO)
        {
            return Task.Run(async () => await TryToCreateLog(apiTrackerDTO));
        }

        private async Task TryToCreateLog(ApiTrackerDTO apiTrackerDTO)
        {
            try
            {
                ITrackingRepository trackingRepository = GetScopedTrackingRepository();
                int retriesCount = _maxAttempts;
                while (retriesCount > 0)
                {
                    /// Reason for this custom implementation:
                    /// - In production BD server stops after a time and it could take a while to start up again
                    /// - This operation runs with fire and forget so it's not blocking the request
                    /// - Hence it can wait much longer than normal requests
                    /// - I want this information to be saved
                    try
                    {
                        await trackingRepository.Create(apiTrackerDTO);
                        retriesCount = 0;
                    }
                    catch (InvalidOperationException ex)
                    {
                        int attempt = _maxAttempts - retriesCount + 1;
                        _logger.LogWarning(ex, "Attempt #{count} failed to save [apiTracker] in DB.", attempt);

                        retriesCount--;
                        if (retriesCount > 0)
                        {
                            // Default delay after:
                            // #1 - 30 sec
                            // #2 - 60 sec
                            await Task.Delay(attempt * RetryDelay);
                        }
                        else
                        {
                            // #3 - error
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("API Tracking Info: {apiTracker}", apiTrackerDTO);
                _logger.LogCritical(ex, "Failed to save [apiTracker] in DB.");
            }
        }

        private ITrackingRepository GetScopedTrackingRepository()
        {
            IServiceScope scope = _serviceScopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ITrackingRepository>();
        }
    }
}
