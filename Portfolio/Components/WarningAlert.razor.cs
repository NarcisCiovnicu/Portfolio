using Microsoft.AspNetCore.Components;

namespace Portfolio.Components
{
    public partial class WarningAlert
    {
        [Parameter, EditorRequired]
        public required string Message { get; set; }
    }
}