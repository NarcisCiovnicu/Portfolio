using System.Text.Json.Serialization;

namespace Portfolio.Models
{
    public class AuthToken
    {
        public required string Value { get; set; }
        public DateTime ExpiresAtUtc { get; set; }

        [JsonIgnore]
        public bool IsExpired { get { return DateTime.UtcNow > ExpiresAtUtc; } }
    }
}
