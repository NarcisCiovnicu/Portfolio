using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Portfolio.Models;
using Portfolio.Services;
using System.Security.Claims;
using System.Text.Json;

namespace Portfolio.Providers
{
    public class ClientAuthStateProvider(IAuthTokenService tokenService) : AuthenticationStateProvider
    {
        private readonly IAuthTokenService _tokenService = tokenService;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthToken? token = await _tokenService.TryGetAuthTokenAsync().ConfigureAwait(false);
            if (token?.IsExpired ?? false)
            {
                await _tokenService.TryRemoveAuthTokenAsync();
                token = null;
            }

            ClaimsIdentity identity = token is null
                ? new()
                : new(ParseClaimsFromJwt(token.Value), "Jwt-Bearer-Token");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void MarkUserAsAuthenticated()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = WebEncoders.Base64UrlDecode(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? ""));
        }
    }
}
