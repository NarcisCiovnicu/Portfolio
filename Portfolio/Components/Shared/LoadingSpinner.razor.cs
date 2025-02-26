using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Portfolio.Components.Shared
{
    public partial class LoadingSpinner
    {
        [Parameter]
        public string? Message { get; set; }
        [Parameter]
        public Color Color { get; set; } = Color.Info;
    }
}