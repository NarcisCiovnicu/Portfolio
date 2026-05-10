using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.API.DataAccess.DatabaseEntities;

public abstract class BaseEntity
{
    [Key, Column(Order = 0)]
    public required Guid Id { get; set; }
}
