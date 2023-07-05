using System.ComponentModel.DataAnnotations;

namespace CookWithMe.Data.Entities;

public class User : EntityBase
{
    [Key]
    public Guid Id { get; set; }

    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? Nickname { get; set; }

    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public string PasswordSalt { get; set; } = null!;
}