using Microsoft.Extensions.Options;
using Portfolio.Models;
using Portfolio.Models.Enums;
using Portfolio.Models.Responses;
using Portfolio.Services;

namespace Portfolio.Pages
{
    public partial class ViewCurriculumVitae(ICurriculumVitaeService cvService, IOptions<ClientAppConfig> appConfig)
    {
        private readonly ICurriculumVitaeService _cvService = cvService;

        protected readonly CVDesignType CvDesignType = appConfig.Value.CVDesignType;

        protected bool IsCvLoading = false;
        protected CurriculumVitae? CurriculumVitae = null;
        protected ProblemDetails? Error = null;
        protected string? WarningMessage = null;

        protected override async Task OnInitializedAsync()
        {
            IsCvLoading = true;
            (CurriculumVitae, Error, WarningMessage) = await _cvService.GetCVAsync();
            IsCvLoading = false;
        }
    }
}
