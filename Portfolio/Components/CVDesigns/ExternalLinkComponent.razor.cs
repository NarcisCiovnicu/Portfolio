using Microsoft.AspNetCore.Components;
using Portfolio.Models;

namespace Portfolio.Components.CVDesigns
{
    public partial class ExternalLinkComponent
    {
        [Parameter, EditorRequired]
        public required Link ExternalLink { get; init; }
    }
}
