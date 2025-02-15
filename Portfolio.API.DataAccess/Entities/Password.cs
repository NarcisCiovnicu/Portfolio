using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.DataAccess.Entities
{
    public class Password
    {
        [Key, MaxLength(64)]
        public required string HashValue { get; set; }
    }
}
