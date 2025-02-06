namespace Portfolio.API.Domain.DataTransferObjects
{
    public record ApiTrackerDTO(
        string IpAddress,
        string RoutePath,
        string? UserAgent,
        string? Country,
        string? City,
        string? ZipCode,
        double? Latitude,
        double? Longitude,
        string? InternetProvider,
        bool? IsMobile,
        bool? IsProxy,
        string? IpLocationError
    );
}
