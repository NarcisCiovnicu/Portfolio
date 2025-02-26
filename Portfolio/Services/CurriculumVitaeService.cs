using Blazored.LocalStorage;
using Portfolio.Models;
using Portfolio.Models.Responses;
using Portfolio.Utils;

namespace Portfolio.Services
{
    public interface ICurriculumVitaeService
    {
        Task<ResponseWithWarning<CurriculumVitae>> GetCVAsync(CancellationToken cancellationToken);
        Task<Response<bool, ProblemDetails>> SaveCVAsync(CurriculumVitae cv);
    }

    internal class CurriculumVitaeService(ILogger<CurriculumVitaeService> logger, HttpClient httpClient, ILocalStorageService localStorage)
        : BaseApiService<ProblemDetails>(logger, httpClient), ICurriculumVitaeService
    {
        private readonly ILogger<CurriculumVitaeService> _logger = logger;
        private readonly ILocalStorageService _localStorage = localStorage;

        public async Task<ResponseWithWarning<CurriculumVitae>> GetCVAsync(CancellationToken cancellationToken)
        {
            CurriculumVitae? cachedCV = await TryToGetFromLocalStorageAsync().ConfigureAwait(false);

            if (cachedCV != null && cachedCV.LastUpdate == DateHelper.Today())
            {
                return new ResponseWithWarning<CurriculumVitae>(cachedCV);
            }

            Response<CurriculumVitae, ProblemDetails> response = await HttpGetAsync<CurriculumVitae>("cv", cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessful)
            {
                await SaveToLocalStorageAsync(response.Result!).ConfigureAwait(false);
                return new ResponseWithWarning<CurriculumVitae>(response);
            }
            else
            {
                string? warningMessage = cachedCV is not null
                    ? "Failed to retrieved the latest CV version (try again later). Displaying an older version viewed before."
                    : null;
                return new ResponseWithWarning<CurriculumVitae>(cachedCV, response.Error, warningMessage);
            }
        }

        public Task<Response<bool, ProblemDetails>> SaveCVAsync(CurriculumVitae cv)
        {
            // TODO
            throw new NotImplementedException();
        }

        private async Task SaveToLocalStorageAsync(CurriculumVitae cv)
        {
            try
            {
                cv.LastUpdate = DateHelper.Today();
                await _localStorage.SetItemAsync(Constants.LocalStorage.CvKey, cv).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save [CV] to [local storage]");
            }
        }

        private async Task<CurriculumVitae?> TryToGetFromLocalStorageAsync()
        {
            try
            {
                return await _localStorage.GetItemAsync<CurriculumVitae>(Constants.LocalStorage.CvKey).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read [CV] form [local storage]");
                return null;
            }
        }
    }
}
