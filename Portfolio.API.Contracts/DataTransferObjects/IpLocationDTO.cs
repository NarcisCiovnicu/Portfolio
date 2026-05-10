namespace Portfolio.API.Contracts.DataTransferObjects;

public record IpLocationDTO(
    string? Country = null,
    string? City = null,
    string? ZipCode = null,
    double? Latitude = null,
    double? Longitude = null,
    string? InternetProvider = null,
    bool? IsMobile = null,
    bool? IsProxy = null,
    string? ErrorMessage = null
);
