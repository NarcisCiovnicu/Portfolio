using Portfolio.API.Domain.DataTransferObjects;

namespace Portfolio.API.Domain.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<bool> IsValid(AuthenticationDTO authenticationDto);

        string GenerateJwtToken();
    }
}
