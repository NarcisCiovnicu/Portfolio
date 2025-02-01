using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using Portfolio.Errors;

namespace Portfolio.Layout
{
    public partial class MainLayout(IWebAssemblyHostEnvironment hostEnvironment, ILogger<MainLayout> logger)
    {
        private readonly IWebAssemblyHostEnvironment _hostEnvironment = hostEnvironment;
        private readonly ILogger<MainLayout> _logger = logger;

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
                IsDarkMode = await MudThemeProvider!.GetSystemPreference();
                StateHasChanged();
            }
        }

        private void ToggleNavMenu()
        {
            IsNavMenuOpen = !IsNavMenuOpen;
        }

        private void CloseNavMenu()
        {
            IsNavMenuOpen = false;
        }
    }
}