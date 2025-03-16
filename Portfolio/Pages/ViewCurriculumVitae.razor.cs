using MudBlazor;
using Portfolio.Components.Alerts;
using Portfolio.Models;
using Portfolio.Models.Responses;
using Portfolio.Services;

namespace Portfolio.Pages
{
    public partial class ViewCurriculumVitae(ICurriculumVitaeService cvService, ISnackbar snackbar) : IDisposable
    {
        private readonly ICurriculumVitaeService _cvService = cvService;
        private readonly ISnackbar _snackbar = snackbar;

        protected CancellationTokenSource? CancellationTokenSource;
        protected bool IsCvLoading = false;
        protected CurriculumVitae? CurriculumVitae = null;
        protected ProblemDetails? Error = null;
        protected string? WarningMessage = null;

        protected override async Task OnInitializedAsync()
        {
            await LoadCVAsync();
        }

        protected async Task LoadCVAsync()
        {
            CurriculumVitae = null;
            Error = null;
            WarningMessage = null;

            CancellationTokenSource = new();

            IsCvLoading = true;
            (CurriculumVitae, Error, WarningMessage) = await _cvService.GetCVAsync(CancellationTokenSource.Token);
            IsCvLoading = false;

            DisplayErrorAsToastWhenThereIsAWarning();
        }

        private void DisplayErrorAsToastWhenThereIsAWarning()
        {
            if (WarningMessage is not null && Error is not null)
            {
                _snackbar.Add<CustomToast>(
                    new Dictionary<string, object>()
                    {
                        { "Title", Error.Title },
                        { "Message", Error.Detail }
                    },
                    Severity.Error);
            }
        }

        public void Dispose()
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
