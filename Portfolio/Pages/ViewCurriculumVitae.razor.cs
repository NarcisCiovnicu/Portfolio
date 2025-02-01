using Microsoft.Extensions.Options;
using Portfolio.Models;
using Portfolio.Models.Enums;
using Portfolio.Services;

namespace Portfolio.Pages
{
    public partial class ViewCurriculumVitae(ICurriculumVitaeService cvService, IOptions<ClientAppConfig> appConfig)
    {
        private readonly ICurriculumVitaeService _cvService = cvService;
        private readonly CVDesignType _cvDesignType = appConfig.Value.CVDesignType;

        private bool _isCvLoading = false;
        private CurriculumVitae? _curriculumVitae = null;
        private string? _errorMessage = null;
        private string? _warningMessage = null;

        protected override async Task OnInitializedAsync()
        {
            _isCvLoading = true;
            (_curriculumVitae, _errorMessage, _warningMessage) = await _cvService.GetCVAsync();
            _isCvLoading = false;
        }
    }
}
