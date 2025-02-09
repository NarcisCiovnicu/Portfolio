using Microsoft.AspNetCore.Components;
using Portfolio.Models;

namespace Portfolio.Components.CVDesigns.One
{
    public partial class RenderProjects
    {
        [Parameter, EditorRequired]
        public required IList<PersonalProject> PersonalProjects { get; init; }
    }
}
