using Microsoft.AspNetCore.Components;

namespace Portfolio.Components
{
    public partial class ErrorAlert
    {
        [Parameter]
        public string? Title { get; set; } = "Sorry, something went wrong 🙈";
        [Parameter, EditorRequired]
        public required string Message { get; set; }
    }
}