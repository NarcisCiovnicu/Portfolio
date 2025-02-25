using Portfolio.Models;
using Portfolio.Services;
using System.Net.Http.Headers;

namespace Portfolio.Handlers
{
    public class AuthorizationHandler(IAuthTokenService tokenService) : DelegatingHandler(new HttpClientHandler())
    {
        private static readonly HashSet<string> _authRoutes = ["POST /api/cv"];

        private readonly IAuthTokenService _tokenService = tokenService;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await TryAddAuthorizationHeader(request, cancellationToken).ConfigureAwait(false);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask TryAddAuthorizationHeader(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestPath = $"{request.Method.Method} {request.RequestUri?.AbsolutePath}";

            if (_authRoutes.Contains(requestPath))
            {
                AuthToken? authToken = await _tokenService.TryGetAuthTokenAsync().ConfigureAwait(false);
                if (authToken is not null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Value);
                }
            }
        }
    }
}
