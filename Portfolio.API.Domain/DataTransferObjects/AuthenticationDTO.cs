using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Domain.DataTransferObjects
{
    public record AuthenticationDTO([Required, MaxLength(30)] string Password);
}
