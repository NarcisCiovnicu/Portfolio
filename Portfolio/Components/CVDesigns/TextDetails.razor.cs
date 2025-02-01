using Microsoft.AspNetCore.Components;

namespace Portfolio.Components.CVDesigns
{
    public partial class TextDetails
    {
        [Parameter, EditorRequired]
        public string? Details { get; set; }
    }
}