using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Domain.DataTransferObjects
{
    public record AuthenticationDTO([Required] string Password);
}
