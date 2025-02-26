using Microsoft.Extensions.Options;
using Portfolio.Models;
using Portfolio.Models.Enums;
using Portfolio.Models.Responses;
using Portfolio.Services;

namespace Portfolio.Pages
{
    public partial class ViewCurriculumVitae(ICurriculumVitaeService cvService, IOptions<ClientAppConfig> appConfig) : IDisposable
    {
        private readonly ICurriculumVitaeService _cvService = cvService;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        protected readonly CVDesignType CvDesignType = appConfig.Value.CVDesignType;

        protected bool IsCvLoading = false;
        protected CurriculumVitae? CurriculumVitae = null;
        protected ProblemDetails? Error = null;
        protected string? WarningMessage = null;

        protected override async Task OnInitializedAsync()
        {
            IsCvLoading = true;
            (CurriculumVitae, Error, WarningMessage) = await _cvService.GetCVAsync(_cancellationTokenSource.Token);
            IsCvLoading = false;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
