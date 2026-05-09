using Microsoft.Extensions.Logging;
using Portfolio.API.Contracts.Constants;
using Portfolio.API.Contracts.CustomExceptions;
using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.Contracts.ServiceInterfaces;
using System.Net;
using System.Net.Http.Json;

namespace Portfolio.API.AppLogic.Services;

internal class IpLocationService(ILogger<IpLocationService> logger, IHttpClientFactory httpClientFactory) : IIpLocationService
{
    private const string REQ_FIELDS = "message,country,city,zip,lat,lon,isp,mobile,proxy";

    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(ConstHttpClientNames.IpLocationApi);
    private readonly ILogger<IpLocationService> _logger = logger;

    public async Task<IpLocationResponseDTO> GetLocation(string ip)
    {
        try
        {
            IpLocationResponseDTO? ipLocation = await _httpClient.GetFromJsonAsync<IpLocationResponseDTO>($"{ip}?fields={REQ_FIELDS}");

            return ipLocation ?? throw new ApiException(HttpStatusCode.NotFound, $"{nameof(IpLocationResponseDTO)} was null");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Request to get location by ip failed.");
            return new IpLocationResponseDTO(ErrorMessage: ex.Message);
        }
    }
}
