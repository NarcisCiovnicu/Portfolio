using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Contracts.DataTransferObjects;

public record AuthenticationDTO([Required, MaxLength(30)] string Password);
