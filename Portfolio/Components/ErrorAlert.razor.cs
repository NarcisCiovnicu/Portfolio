using Microsoft.AspNetCore.Components;
using Portfolio.Models.Responses;

namespace Portfolio.Components
{
    public partial class ErrorAlert
    {
        [Parameter, EditorRequired]
        public required ProblemDetails Error { get; set; }

        [Parameter]
        public string? Class { get; set; }
    }
}