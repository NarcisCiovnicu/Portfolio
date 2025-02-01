using Microsoft.AspNetCore.Components;
using Portfolio.Models;

namespace Portfolio.Components.CVDesigns
{
    public abstract partial class BaseRenderDesign
    {
        [Parameter, EditorRequired]
        public required CurriculumVitae CurriculumVitae { get; set; }
    }
}