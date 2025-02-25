using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Portfolio.Models;
using Portfolio.Models.Responses;
using Portfolio.Providers;
using Portfolio.Services;

namespace Portfolio.Pages
{
    public partial class Login(IAuthService authService, ClientAuthStateProvider authStateProvider, NavigationManager navManager)
    {
        [CascadingParameter]
        public Task<AuthenticationState>? AuthenticationState { get; set; }

        private readonly IAuthService _authService = authService;
        private readonly ClientAuthStateProvider _authStateProvider = authStateProvider;
        private readonly NavigationManager _navManager = navManager;

        protected LoginModel LoginModel = new();
        protected MudForm? MudForm;
        protected bool IsAuthenticating = false;

        protected bool IsAuthenticated = false;
        protected ProblemDetails? Error = null;

        protected async Task Submit()
        {
            await MudForm!.Validate();

            if (MudForm.IsValid && IsAuthenticating == false)
            {
                IsAuthenticating = true;
                Error = null;

                (IsAuthenticated, Error) = await _authService.LoginAsync(LoginModel);
                IsAuthenticating = false;

                if (IsAuthenticated)
                {
                    _authStateProvider.MarkUserAsAuthenticated();
                    _navManager.NavigateTo("/cv/edit");
                }
            }
        }

        public async Task Enter(KeyboardEventArgs e)
        {
            if (IsAuthenticating == false && e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await Submit();
            }
        }
    }
}
