using System.ComponentModel.DataAnnotations;

namespace CookWithMe.Data.Entities;

public class EntityBase
{
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required]
    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    public DateTime? DeletedAt { get; set; }
}