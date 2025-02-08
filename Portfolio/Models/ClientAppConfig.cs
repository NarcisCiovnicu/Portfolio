using Portfolio.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class ClientAppConfig
    {
        [Required, Url]
        public required string ApiUrl { get; init; }
        [Required]
        public required string OwnerName { get; init; }
        public CVDesignType CVDesignType { get; init; } = CVDesignType.Default;
        [Required, EmailAddress]
        public required string EmailContact { get; init; }
    }
}
