using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.DataAccess.DatabaseEntities;

public class Password
{
    [Key, MaxLength(64)]
    public required string HashValue { get; set; }
}
