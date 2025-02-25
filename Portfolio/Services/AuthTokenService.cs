using Blazored.LocalStorage;
using Portfolio.Models;

namespace Portfolio.Services
{
    public interface IAuthTokenService
    {
        Task<AuthToken?> TryGetAuthTokenAsync();
        Task<bool> TryRemoveAuthTokenAsync();
        ValueTask SaveAuthTokenAsync(AuthToken authToken);
    }

    internal class AuthTokenService(ILogger<AuthTokenService> logger, ILocalStorageService localStorage) : IAuthTokenService
    {
        private readonly ILogger<AuthTokenService> _logger = logger;
        private readonly ILocalStorageService _localStorage = localStorage;

        public ValueTask SaveAuthTokenAsync(AuthToken authToken)
        {
            return _localStorage.SetItemAsync(Constants.LocalStorage.AccessTokenKey, authToken);
        }

        public async Task<AuthToken?> TryGetAuthTokenAsync()
        {
            try
            {
                return await _localStorage.GetItemAsync<AuthToken>(Constants.LocalStorage.AccessTokenKey).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get [auth token] from [local storage].");
                return null;
            }
        }

        public async Task<bool> TryRemoveAuthTokenAsync()
        {
            try
            {
                await _localStorage.RemoveItemAsync(Constants.LocalStorage.AccessTokenKey).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove [auth token] from [local storage].");
                return false;
            }
        }
    }
}
