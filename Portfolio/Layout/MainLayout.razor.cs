using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using Portfolio.Errors;
using Portfolio.Services;

namespace Portfolio.Layout
{
    public partial class MainLayout(IWebAssemblyHostEnvironment hostEnvironment, ILogger<MainLayout> logger, IAppThemeService appThemeService)
    {
        private readonly IWebAssemblyHostEnvironment _hostEnvironment = hostEnvironment;
        private readonly ILogger<MainLayout> _logger = logger;
        private readonly IAppThemeService _appThemeService = appThemeService;

        private bool IsNavMenuOpen { get; set; } = false;
        private bool IsDarkMode { get; set; } = false;
        private string ThemeName
        {
            get { return IsDarkMode ? "Dark" : "Light"; }
        }

        private MudThemeProvider? MudThemeProvider { get; set; }
        private GlobalErrorBoundary? GlobalErrorBoundary { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _logger.LogInformation("Current environment: {Environment}", _hostEnvironment.Environment);
                await SetTheme();
                StateHasChanged();
            }
        }

        private async Task DarkModeChanged(bool isDarkMode)
        {
            IsDarkMode = isDarkMode;
            await _appThemeService.SaveDarkMode(isDarkMode);
        }

        private void ToggleNavMenu()
        {
            IsNavMenuOpen = !IsNavMenuOpen;
        }

        private void CloseNavMenu()
        {
            IsNavMenuOpen = false;
        }

        private async Task SetTheme()
        {
            bool? savedDarkMode = await _appThemeService.IsDarkModePreferred();
            if (savedDarkMode == null)
            {
                IsDarkMode = await MudThemeProvider!.GetSystemPreference();
            }
            else
            {
                IsDarkMode = savedDarkMode.Value;
            }
        }
    }
}