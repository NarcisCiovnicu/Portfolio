using MudBlazor;
using Portfolio.Errors;

namespace Portfolio.Layout
{
    public partial class MainLayout()
    {
        private bool _isNavMenuOpen { get; set; } = false;
        private bool _isDarkMode { get; set; } = false;
        private string _themeName
        {
            get { return _isDarkMode ? "Dark" : "Light"; }
        }

        private MudThemeProvider? _mudThemeProvider { get; set; }
        private GlobalErrorBoundary? _globalErrorBoundary { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _isDarkMode = await _mudThemeProvider!.GetSystemPreference();
                StateHasChanged();
            }
        }

        private void ToggleNavMenu()
        {
            _isNavMenuOpen = !_isNavMenuOpen;
        }

        private void CloseNavMenu()
        {
            _isNavMenuOpen = false;
        }
    }
}