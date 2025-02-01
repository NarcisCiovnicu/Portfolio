using Microsoft.AspNetCore.Components;
using Portfolio.Models;

namespace Portfolio.Components.CVDesigns.One
{
    public partial class RenderStartSection
    {
        [Parameter, EditorRequired]
        public required CurriculumVitae CurriculumVitae { get; set; }
    }
}
