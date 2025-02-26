using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Portfolio.Components.Alerts
{
    public partial class WarningAlert
    {
        [Parameter, EditorRequired]
        public required string Message { get; set; }
        [Parameter]
        public EventCallback OnCloseEvent { get; set; }
        [Parameter]
        public string CloseIcon { get; set; } = Icons.Material.Filled.Close;
    }
}