using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.ServiceInterfaces;
using System.Net;

namespace Portfolio.API.Middlewares
{
    public class TrackingMiddleware(RequestDelegate next, ILogger<TrackingMiddleware> logger, IIpLocationService ipLocationService, ITrackingService trackingService)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<TrackingMiddleware> _logger = logger;
        private readonly IIpLocationService _ipLocationService = ipLocationService;
        private readonly ITrackingService _trackingService = trackingService;

        public async Task InvokeAsync(HttpContext context)
        {
            Task task = LogTrackingInformation(context);

            await _next(context);

            await task;
        }

        private async Task LogTrackingInformation(HttpContext context)
        {
            IPAddress ipAddress = GetIp(context);
            string? userAgent = context.Request.Headers.UserAgent;
            string path = $"{context.Request.Method} - {context.Request.Path}";
            IpLocationResponseDTO ipLocation;

            bool isAnyIp = ipAddress == IPAddress.Any;
            if (IPAddress.IsLoopback(ipAddress) || isAnyIp)
            {
                string ipType = isAnyIp ? "Any" : "Loopback";
                ipLocation = new IpLocationResponseDTO(ErrorMessage: $"Ip address is {ipType}");
            }
            else
            {
                ipLocation = await _ipLocationService.GetLocation(ipAddress.ToString());
            }

            ApiTrackerDTO apiTracker = new(ipAddress.ToString(), path, userAgent, ipLocation.Country, ipLocation.City, ipLocation.ZipCode,
                ipLocation.Latitude, ipLocation.Longitude, ipLocation.InternetProvider, ipLocation.IsMobile, ipLocation.IsProxy, ipLocation.ErrorMessage);

            _ = _trackingService.LogWithFireAndForget(apiTracker);

            _logger.LogInformation("Tracking Info: {apiTracker}", apiTracker);
        }

        private static IPAddress GetIp(HttpContext context)
        {
            string? forwarded = context.Request.Headers["HTTP_X_FORWARDED_FOR"];
            if (IPAddress.TryParse(forwarded, out IPAddress? ip))
            {
                return ip;
            }

            return context.Connection.RemoteIpAddress ?? IPAddress.Any;
        }
    }
}
