using Microsoft.AspNetCore.Components;
using Portfolio.Models;

namespace Portfolio.Components.CVDesigns.One
{
    public partial class RenderSkills
    {
        [Parameter, EditorRequired]
        public required IList<Skill> Skills { get; set; }
    }
}