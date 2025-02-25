using Blazored.LocalStorage.Exceptions;
using MudBlazor;
using Portfolio.Models;
using Portfolio.Models.Responses;

namespace Portfolio.Services
{
    public interface IAuthService
    {
        Task<Response<bool, ProblemDetails>> LoginAsync(LoginModel loginForm);
        Task<bool> LogOutAsync();
    }

    internal class AuthService(ILogger<AuthService> logger, IAuthTokenService authTokenService, HttpClient httpClient, ISnackbar snackbar)
        : BaseApiService<ProblemDetails>(logger, httpClient), IAuthService
    {
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IAuthTokenService _authTokenService = authTokenService;
        private readonly ISnackbar _snackbar = snackbar;

        public async Task<Response<bool, ProblemDetails>> LoginAsync(LoginModel loginForm)
        {
            Response<AuthToken, ProblemDetails> response = await HttpPostAsync<LoginModel, AuthToken>("authenticate", loginForm).ConfigureAwait(false);

            if (response.IsSuccessful)
            {
                bool wasSaved = await TryToSaveTokenAsync(response.Result!).ConfigureAwait(false);
                return new Response<bool, ProblemDetails>(wasSaved);
            }
            else
            {
                return new Response<bool, ProblemDetails>(false, response.Error);
            }
        }

        public async Task<bool> LogOutAsync()
        {
            bool wasRemoved = await _authTokenService.TryRemoveAuthTokenAsync().ConfigureAwait(false);

            if (wasRemoved is false)
            {
                _snackbar.Add("Unexpected error. Failed to log out.", Severity.Error);
            }

            return wasRemoved;
        }

        private async Task<bool> TryToSaveTokenAsync(AuthToken authToken)
        {
            try
            {
                await _authTokenService.SaveAuthTokenAsync(authToken).ConfigureAwait(false);
                return true;
            }
            catch (BrowserStorageDisabledException)
            {
                _snackbar.Add("Local storage is disabled for this website/browser/device. Enable local storage or login from another browser/device.", Severity.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save auth token to [local storage].");
                _snackbar.Add("Critical erorr. Failed to save authentication token on this device.", Severity.Error);
            }

            return false;
        }
    }
}
