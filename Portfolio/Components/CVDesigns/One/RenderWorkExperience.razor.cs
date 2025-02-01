using Microsoft.AspNetCore.Components;
using Portfolio.Models;

namespace Portfolio.Components.CVDesigns.One
{
    public partial class RenderWorkExperience
    {
        [Parameter, EditorRequired]
        public required IList<WorkExperience> WorkExperienceList { get; set; }
    }
}