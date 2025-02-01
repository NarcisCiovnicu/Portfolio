using Microsoft.AspNetCore.Components;

namespace Portfolio.Pages
{
    public partial class Home(NavigationManager navManager)
    {
        private readonly NavigationManager _navManager = navManager;

        protected override void OnInitialized()
        {
            _navManager.NavigateTo("/cv");
        }
    }
}