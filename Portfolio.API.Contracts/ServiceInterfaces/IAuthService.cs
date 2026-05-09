using Portfolio.API.Contracts.DataTransferObjects;

namespace Portfolio.API.Contracts.ServiceInterfaces;

public interface IAuthService
{
    Task<bool> IsValid(AuthenticationDTO authenticationDTO, CancellationToken cancellationToken);

    AuthTokenDTO GenerateJwtToken();
}
