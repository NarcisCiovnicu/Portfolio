using Microsoft.AspNetCore.Components;

namespace Portfolio.Components.CVDesigns.One
{
    public partial class RenderSection
    {
        [Parameter, EditorRequired]
        public required string Title { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}