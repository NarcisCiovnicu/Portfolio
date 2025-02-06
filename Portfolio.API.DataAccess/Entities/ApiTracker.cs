using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.API.DataAccess.Entities
{
    [Table("Tracking")]
    public class ApiTracker : BaseEntity<Guid>
    {
        [Required, MaxLength(64)]
        public required string IpAddress { get; set; }
        [Required, MaxLength(64)]
        public required string RoutePath { get; set; }
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? UserAgent { get; set; }
        [MaxLength(32)]
        public string? Country { get; set; }
        [MaxLength(32)]
        public string? City { get; set; }
        [MaxLength(16)]
        public string? ZipCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        [MaxLength(32)]
        public string? InternetProvider { get; set; }
        public bool? IsMobile { get; set; }
        public bool? IsProxy { get; set; }
        [MaxLength(512)]
        public string? IpLocationError { get; set; }
    }
}
