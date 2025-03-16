using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using Portfolio.Errors;
using Portfolio.Providers;
using Portfolio.Services;
using System.Reflection;

namespace Portfolio.Layout
{
    public partial class MainLayout(IWebAssemblyHostEnvironment hostEnvironment, ILogger<MainLayout> logger, IAppThemeService appThemeService,
        IAuthService authService, ClientAuthStateProvider authStateProvider, NavigationManager navManager)
    {
        private readonly IWebAssemblyHostEnvironment _hostEnvironment = hostEnvironment;
        private readonly ILogger<MainLayout> _logger = logger;
        private readonly IAppThemeService _appThemeService = appThemeService;
        private readonly IAuthService _authService = authService;
        private readonly ClientAuthStateProvider _authStateProvider = authStateProvider;
        private readonly NavigationManager _navManager = navManager;

        protected static string? AppVersion => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

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

        protected async Task LogOut()
        {
            bool wasLoggedOut = await _authService.LogOutAsync();
            if (wasLoggedOut)
            {
                _authStateProvider.MarkUserAsLoggedOut();
                _navManager.NavigateTo("/");
            }
        }

        private async Task DarkModeChanged(bool isDarkMode)
        {
            IsDarkMode = isDarkMode;
            await _appThemeService.SaveDarkModeAsync(isDarkMode);
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
            bool? savedDarkMode = await _appThemeService.IsDarkModePreferredAsync();
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