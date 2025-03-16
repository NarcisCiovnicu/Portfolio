using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Portfolio.Models;
using Portfolio.Models.Enums;

namespace Portfolio.Components
{
    public partial class RenderCV(IOptions<ClientAppConfig> appConfig)
    {
        [Parameter, EditorRequired]
        public required CurriculumVitae CurriculumVitae { get; set; }

        protected readonly CVDesignType CvDesignType = appConfig.Value.CVDesignType;
    }
}