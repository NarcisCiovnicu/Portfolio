using Blazored.LocalStorage;
using Blazored.LocalStorage.Exceptions;
using MudBlazor;

namespace Portfolio.Services
{
    public interface IAppThemeService
    {
        Task<bool?> IsDarkModePreferredAsync();
        Task SaveDarkModeAsync(bool isDarkMode);
    }

    public class AppThemeService(ILocalStorageService localStorage, ILogger<AppThemeService> logger, ISnackbar snackbar) : IAppThemeService
    {
        private readonly ILocalStorageService _localStorage = localStorage;
        private readonly ILogger<AppThemeService> _logger = logger;
        private readonly ISnackbar _snackbar = snackbar;

        public async Task<bool?> IsDarkModePreferredAsync()
        {
            try
            {
                return await _localStorage.GetItemAsync<bool?>(Constants.LocalStorage.IsDarkThemeKey).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read [theme] from [local storage]");
                return null;
            }
        }

        public async Task SaveDarkModeAsync(bool isDarkMode)
        {
            try
            {
                await _localStorage.SetItemAsync(Constants.LocalStorage.IsDarkThemeKey, isDarkMode).ConfigureAwait(false);
            }
            catch (BrowserStorageDisabledException)
            {
                _snackbar.Add("Failed to save preferred theme. Local storage is disabled.", Severity.Warning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save [theme] to [local storage]");
            }
        }
    }
}
