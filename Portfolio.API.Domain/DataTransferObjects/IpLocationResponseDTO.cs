using System.Text.Json.Serialization;

namespace Portfolio.API.Domain.DataTransferObjects
{
    public record IpLocationResponseDTO(
        [property: JsonPropertyName("message")] string? ErrorMessage,
        string? Country = null,
        string? City = null,
        [property: JsonPropertyName("zip")] string? ZipCode = null,
        [property: JsonPropertyName("lat")] double? Latitude = null,
        [property: JsonPropertyName("lon")] double? Longitude = null,
        [property: JsonPropertyName("isp")] string? InternetProvider = null,
        [property: JsonPropertyName("proxy")] bool? IsMobile = null,
        [property: JsonPropertyName("mobile")] bool? IsProxy = null
    );
}
