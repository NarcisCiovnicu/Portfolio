using System.Text.Json.Serialization;

namespace Portfolio.API.DataAccess.ProviderModels;

internal record IpLocationResponse(
    [property: JsonPropertyName("country")] string? Country,
    [property: JsonPropertyName("regionName")] string? Region,
    [property: JsonPropertyName("city")] string? City,
    [property: JsonPropertyName("zip")] string? ZipCode,
    [property: JsonPropertyName("lat")] double? Latitude,
    [property: JsonPropertyName("lon")] double? Longitude,
    [property: JsonPropertyName("isp")] string? InternetProvider,
    [property: JsonPropertyName("mobile")] bool? IsMobile,
    [property: JsonPropertyName("proxy")] bool? IsProxy,
    [property: JsonPropertyName("message")] string? ErrorMessage
);
