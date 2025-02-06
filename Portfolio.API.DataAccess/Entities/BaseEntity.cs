using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.API.DataAccess.Entities
{
    public abstract class BaseEntity<T>
    {
        [Key, Column(Order = 0)]
        public required T Id { get; set; }
    }
}
