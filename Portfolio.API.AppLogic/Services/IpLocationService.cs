using Microsoft.Extensions.Logging;
using Portfolio.API.Domain;
using Portfolio.API.Domain.CustomExceptions;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.ServiceInterfaces;
using System.Net.Http.Json;

namespace Portfolio.API.AppLogic.Services
{
    internal class IpLocationService(ILogger<IpLocationService> logger, IHttpClientFactory httpClientFactory) : IIpLocationService
    {
        private const string IncludeFields = "message,country,city,zip,lat,lon,isp,mobile,proxy";

        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Constants.IpLocationApi.Name);
        private readonly ILogger<IpLocationService> _logger = logger;

        public async Task<IpLocationResponseDTO> GetLocation(string ip)
        {
            try
            {
                IpLocationResponseDTO? ipLocation = await _httpClient.GetFromJsonAsync<IpLocationResponseDTO>($"{ip}?fields={IncludeFields}").ConfigureAwait(false);

                return ipLocation ?? throw new ApiException($"{nameof(IpLocationResponseDTO)} was null");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request to get location by ip failed.");
                return new IpLocationResponseDTO(ErrorMessage: ex.Message);
            }
        }
    }
}
