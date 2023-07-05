using System.ComponentModel.DataAnnotations;

namespace CookWithMe.Data.Entities;

public class Meal
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public MealType MealType { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Required]
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}