using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Portfolio.API.AppLogic.Services;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.RepositoryInterfaces;

namespace Portfolio.API.AppLogic.Test.Services
{
    public class TrackingServiceTests
    {
        private readonly Mock<ILogger<TrackingService>> _mockLogger = new();
        private readonly Mock<ITrackingRepository> _mockTrackingRepo = new();
        private readonly Mock<IServiceScopeFactory> _mockServiceScopeFactory = new();

        private readonly ApiTrackerDTO _mockApiTrackerDTO = new("1.1.1.1", "a", "a", "a", "a", "a", 1.2, 1.2, "a", false, false, "a");

        #region RetryDelay
        [Theory]
        [InlineData(0, 1)]
        [InlineData(-5, 1)]
        [InlineData(50, 50)]
        [InlineData(60_000, 60_000)]
        [InlineData(60_001, 60_000)]
        public void RetryDelay_ShouldClampTheValueBetweenOneMilisecondAnd60Seconds(int input, int expected)
        {
            // setup & execute
            TrackingService trackingService = new(_mockLogger.Object, _mockServiceScopeFactory.Object, input);

            // assert
            Assert.Equal(expected, trackingService.RetryDelay);
        }
        #endregion

        #region LogWithFireAndForget
        [Fact]
        public async Task LogWithFireAndForget_ShouldCreateApiTrackerInRepo()
        {
            // setup
            SetupServiceFactory();
            TrackingService trackingService = new(_mockLogger.Object, _mockServiceScopeFactory.Object);

            // execute
            Task serviceTask = trackingService.LogWithFireAndForget(_mockApiTrackerDTO);
            Task timeout = Task.Delay(100);
            Task finishedTask = await Task.WhenAny(serviceTask, timeout);

            // assert
            if (finishedTask == timeout)
            {
                Assert.Fail("The background task did not complete within the allowed timeout.");
            }
            _mockTrackingRepo.Verify(m => m.Create(_mockApiTrackerDTO), Times.Once);
        }

        [Fact]
        public async Task LogWithFireAndForget_ShouldLogCritErrorAndInfo_WhenExceptionIsThrown()
        {
            // setup
            SetupServiceFactory();
            var mockException = new Exception("mock test");
            _mockTrackingRepo.Setup(m => m.Create(It.IsAny<ApiTrackerDTO>())).Throws(mockException);
            TrackingService trackingService = new(_mockLogger.Object, _mockServiceScopeFactory.Object);

            // execute
            Task serviceTask = trackingService.LogWithFireAndForget(_mockApiTrackerDTO);
            Task timeout = Task.Delay(100);
            Task finishedTask = await Task.WhenAny(serviceTask, timeout);

            // assert
            if (finishedTask == timeout)
            {
                Assert.Fail("The background task did not complete within the allowed timeout.");
            }
            _mockLogger.Verify(
                m => m.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((msg, t) => msg.ToString() == "Failed to save [apiTracker] in DB."),
                    mockException,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
            _mockLogger.Verify(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
        }

        [Fact]
        public async Task LogWithFireAndForget_ShouldRetry3Times_WhenInvalidOperationExceptionIsThrown()
        {
            // setup
            SetupServiceFactory();
            var mockException = new InvalidOperationException("mock test");
            _mockTrackingRepo.Setup(m => m.Create(It.IsAny<ApiTrackerDTO>())).Throws(mockException);
            TrackingService trackingService = new(_mockLogger.Object, _mockServiceScopeFactory.Object, 1);

            // execute
            Task serviceTask = trackingService.LogWithFireAndForget(_mockApiTrackerDTO);
            Task timeout = Task.Delay(100);
            Task finishedTask = await Task.WhenAny(serviceTask, timeout);

            // assert
            if (finishedTask == timeout)
            {
                Assert.Fail("The background task did not complete within the allowed timeout.");
            }
            _mockTrackingRepo.Verify(m => m.Create(_mockApiTrackerDTO), Times.Exactly(3));
        }

        [Fact]
        public async Task LogWithFireAndForget_ShouldLogWarnings_WhenInvalidOperationExceptionIsThrown_AndLogCritErrorAfter()
        {
            // setup
            SetupServiceFactory();
            var mockException = new InvalidOperationException("mock test");
            _mockTrackingRepo.Setup(m => m.Create(It.IsAny<ApiTrackerDTO>())).Throws(mockException);
            TrackingService trackingService = new(_mockLogger.Object, _mockServiceScopeFactory.Object, 1);

            // execute
            Task serviceTask = trackingService.LogWithFireAndForget(_mockApiTrackerDTO);
            Task timeout = Task.Delay(100);
            Task finishedTask = await Task.WhenAny(serviceTask, timeout);

            // assert
            if (finishedTask == timeout)
            {
                Assert.Fail("The background task did not complete within the allowed timeout.");
            }
            _mockLogger.Verify(
                m => m.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    mockException,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Exactly(3)
            );
            _mockLogger.Verify(
                m => m.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    mockException,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
        }

        // Example of test when you don't await a Task
        //[Fact]
        //public async Task LogWithFireAndForget()
        //{
        //    // setup
        //    SetupServiceFactory();
        //    TrackingService trackingService = new(_mockLogger.Object, _mockServiceScopeFactory.Object);
        //    var tcs = new TaskCompletionSource<bool>();

        //    // execute
        //    _ = Task.Run(async () =>
        //    {
        //        _ = trackingService.LogWithFireAndForget(_mockApiTrackerDTO);  // Call the fire-and-forget method
        //        await Task.Delay(1_000);  // Wait for the task to likely complete
        //        tcs.SetResult(true);  // Signal completion
        //    });
        //    var isCompleted = await tcs.Task;

        //    // assert
        //    Assert.True(isCompleted);
        //    _mockTrackingRepo.Verify(m => m.Create(_mockApiTrackerDTO), Times.Once);
        //}
        #endregion

        private void SetupServiceFactory()
        {
            Mock<IServiceProvider> mockServiceProvider = new();
            Mock<IServiceScope> mockScope = new();

            mockServiceProvider.Setup(m => m.GetService(typeof(ITrackingRepository))).Returns(_mockTrackingRepo.Object);
            mockScope.SetupGet(m => m.ServiceProvider).Returns(mockServiceProvider.Object);

            _mockServiceScopeFactory.Setup(m => m.CreateScope()).Returns(mockScope.Object);
        }
    }
}
