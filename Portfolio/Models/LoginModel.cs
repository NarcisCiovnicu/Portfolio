using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class LoginModel
    {
        [Required, MaxLength(30, ErrorMessage = "Password must not exceed 30 characters")]
        public string Password { get; set; } = string.Empty;
    }
}
