using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Portfolio.Errors
{
    public class GlobalErrorBoundary(IWebAssemblyHostEnvironment environment) : ErrorBoundary()
    {
        private readonly IWebAssemblyHostEnvironment _environment = environment;

        protected override Task OnErrorAsync(Exception exception)
        {
            if (_environment.IsDevelopment() || _environment.IsStaging())
            {
                return base.OnErrorAsync(exception);
            }
            return Task.CompletedTask;
        }
    }
}
