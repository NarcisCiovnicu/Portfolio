using Microsoft.AspNetCore.Components;

namespace Portfolio.Components
{
    public partial class LoadingSpinner
    {
        [Parameter]
        public string? Message { get; set; }
    }
}