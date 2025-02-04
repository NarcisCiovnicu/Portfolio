using Blazored.LocalStorage;
using MudBlazor;

namespace Portfolio.Services
{
    public interface IAppThemeService
    {
        Task<bool?> IsDarkModePreferred();
        Task SaveDarkMode(bool isDarkMode);
    }

    public class AppThemeService(ILocalStorageService localStorage, ILogger<AppThemeService> logger, ISnackbar snackbar) : IAppThemeService
    {
        private readonly ILocalStorageService _localStorage = localStorage;
        private readonly ILogger<AppThemeService> _logger = logger;
        private readonly ISnackbar _snackbar = snackbar;

        public async Task<bool?> IsDarkModePreferred()
        {
            try
            {
                return await _localStorage.GetItemAsync<bool?>(Constants.LocalStorage.IsDarkThemeKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read [theme] from [local storage]");
                return null;
            }
        }

        public async Task SaveDarkMode(bool isDarkMode)
        {
            try
            {
                await _localStorage.SetItemAsync(Constants.LocalStorage.IsDarkThemeKey, isDarkMode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save [theme] to [local storage]");
                _snackbar.Add("Failed to save preferred theme.", Severity.Error);
            }
        }
    }
}
