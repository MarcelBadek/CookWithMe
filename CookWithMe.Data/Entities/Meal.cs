using System.ComponentModel.DataAnnotations;

namespace CookWithMe.Data.Entities;

public class Meal : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public MealType MealType { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public Guid UserId { get; set; }
    
    public User User { get; set; } = null!;
}