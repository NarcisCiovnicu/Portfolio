using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.ServiceInterfaces;
using System.Net;

namespace Portfolio.API.Middlewares
{
    public class TrackingMiddleware(RequestDelegate next, ILogger<TrackingMiddleware> logger, IIpLocationService ipLocationService)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<TrackingMiddleware> _logger = logger;
        private readonly IIpLocationService _ipLocationService = ipLocationService;

        public async Task InvokeAsync(HttpContext context, ITrackingService trackingService)
        {
            Task task = LogTrackingInformation(context, trackingService);

            await _next(context);

            await task;
        }

        private async Task LogTrackingInformation(HttpContext context, ITrackingService trackingService)
        {
            IPAddress ipAddress = GetIp(context);
            string? userAgent = context.Request.Headers.UserAgent;
            string path = context.Request.Path;
            IpLocationResponseDTO ipLocation;

            _logger.LogInformation("Request made by {ipAddress}", ipAddress);

            bool isAnyIp = ipAddress == IPAddress.Any;
            if (IPAddress.IsLoopback(ipAddress) || isAnyIp)
            {
                string ipType = isAnyIp ? "Any" : "Loopback";
                ipLocation = new IpLocationResponseDTO(ErrorMessage: $"Ip address is {ipType}");
            }
            else
            {
                ipLocation = await _ipLocationService.GetLocation(ipAddress.ToString());
                _logger.LogInformation("Request from {location}, with error {errorMessage}", $"{ipLocation.Country} {ipLocation.City}", ipLocation.ErrorMessage);
            }

            ApiTrackerDTO apiTracker = new(ipAddress.ToString(), path, userAgent, ipLocation.Country, ipLocation.City, ipLocation.ZipCode,
                ipLocation.Latitude, ipLocation.Longitude, ipLocation.InternetProvider, ipLocation.IsMobile, ipLocation.IsProxy, ipLocation.ErrorMessage);

            await trackingService.Create(apiTracker);
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
