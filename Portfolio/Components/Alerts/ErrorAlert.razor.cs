using Microsoft.AspNetCore.Components;
using MudBlazor;
using Portfolio.Models.Responses;

namespace Portfolio.Components.Alerts
{
    public partial class ErrorAlert
    {
        [Parameter, EditorRequired]
        public required ProblemDetails Error { get; set; }
        [Parameter]
        public string? Class { get; set; }
        [Parameter]
        public EventCallback OnCloseEvent { get; set; }
        [Parameter]
        public string CloseIcon { get; set; } = Icons.Material.Filled.Close;
    }
}
