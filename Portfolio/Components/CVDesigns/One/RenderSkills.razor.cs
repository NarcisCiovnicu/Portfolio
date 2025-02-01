using Microsoft.AspNetCore.Components;

namespace Portfolio.Components.CVDesigns.One
{
    public partial class RenderSkills
    {
        [Parameter, EditorRequired]
        public required IList<string> Skills { get; set; }
    }
}