using Microsoft.AspNetCore.Components;

namespace Portfolio.Components.Alerts
{
    public partial class CustomToast
    {
        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Message { get; set; }
    }
}
