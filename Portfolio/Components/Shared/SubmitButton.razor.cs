using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Portfolio.Components.Shared
{
    public partial class SubmitButton
    {
        [Parameter, EditorRequired]
        public required string Label { get; set; }
        [Parameter]
        public string? SubmittingLabel { get; set; }
        [Parameter]
        public bool IsSubmitting { get; set; } = false;

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter]
        public Color Color { get; set; } = Color.Primary;
        [Parameter]
        public Variant Variant { get; set; } = Variant.Filled;
        [Parameter]
        public bool Disabled { get; set; } = false;
        [Parameter]
        public string? Class { get; set; }
        [Parameter]
        public string? StartIcon { get; set; }
        [Parameter]
        public string? EndIcon { get; set; }
        [Parameter]
        public Size Size { get; set; } = Size.Medium;
    }
}
