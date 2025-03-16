using MudBlazor;
using Portfolio.Components.Alerts;
using Portfolio.Models;
using Portfolio.Models.Responses;
using Portfolio.Services;

namespace Portfolio.Pages
{
    public partial class EditCurriculumVitae(ICurriculumVitaeService cvService, ISnackbar snackbar) : IDisposable
    {
        private readonly ICurriculumVitaeService _cvService = cvService;
        private readonly ISnackbar _snackbar = snackbar;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        protected bool IsCvLoading = false;
        protected bool IsCvSaving = false;
        protected MudForm? MudForm;
        protected CurriculumVitae? CurriculumVitae = null;
        protected ProblemDetails? Error = null;

        protected override async Task OnInitializedAsync()
        {
            IsCvLoading = true;
            (CurriculumVitae, Error) = await _cvService.GetLatestCVAsync(_cancellationTokenSource.Token);
            IsCvLoading = false;
        }

        private async Task SaveCVAsync()
        {
            await MudForm!.Validate();

            if (MudForm.IsValid && IsCvSaving == false)
            {
                IsCvSaving = true;

                Response<CurriculumVitae, ProblemDetails> response = await _cvService.SaveCVAsync(CurriculumVitae!);
                HandleSaveResponse(response);

                IsCvSaving = false;
            }
        }

        private void HandleSaveResponse(Response<CurriculumVitae, ProblemDetails> response)
        {
            if (response.IsSuccessful)
            {
                CurriculumVitae = response.Result!;
            }
            else
            {
                ProblemDetails error = response.Error!;
                _snackbar.Add<CustomToast>(
                    new Dictionary<string, object>()
                    {
                        { "Title", error.Title },
                        { "Message", error.Detail }
                    },
                    Severity.Error);
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
