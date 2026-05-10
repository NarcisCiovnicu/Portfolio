using MapsterMapper;
using Microsoft.Extensions.Logging;
using Portfolio.API.Contracts.Constants;
using Portfolio.API.Contracts.CustomExceptions;
using Portfolio.API.Contracts.DataTransferObjects;
using Portfolio.API.Contracts.ProviderInterfaces;
using Portfolio.API.DataAccess.ProviderModels;
using System.Net;
using System.Net.Http.Json;

namespace Portfolio.API.DataAccess.Providers;

internal class IpLocationProvider(ILogger<IpLocationProvider> logger, IMapper mapper, IHttpClientFactory httpClientFactory)
    : IIpLocationProvider
{
    private const string REQ_FIELDS = "message,country,city,zip,lat,lon,isp,mobile,proxy";

    private readonly ILogger<IpLocationProvider> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(ConstHttpClientNames.IpLocationApi);

    public async Task<IpLocationDTO> GetLocation(string ip)
    {
        try
        {
            IpLocationResponse? ipLocation = await _httpClient.GetFromJsonAsync<IpLocationResponse>($"{ip}?fields={REQ_FIELDS}");

            return ipLocation is null
                ? throw new ApiException(HttpStatusCode.NotFound, $"{nameof(IpLocationResponse)} was null")
                : _mapper.Map<IpLocationDTO>(ipLocation);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Request to get location by ip failed.");
            return new IpLocationDTO(ErrorMessage: ex.Message);
        }
    }
}
