using Microsoft.AspNetCore.Components;

namespace Portfolio.Components
{
    public partial class NavMenu
    {
        [Parameter, EditorRequired]
        public EventCallback OnClose { get; set; }

        private async Task CloseNavMenu()
        {
            await OnClose.InvokeAsync();
        }
    }
}