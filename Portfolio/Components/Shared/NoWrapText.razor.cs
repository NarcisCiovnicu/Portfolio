using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Portfolio.Components.Shared
{
    public partial class NoWrapText : MudComponentBase
    {
        [Parameter, EditorRequired]
        public required string Text { get; set; }
    }
}