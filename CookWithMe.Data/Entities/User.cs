using Microsoft.AspNetCore.Identity;

namespace CookWithMe.Data.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<Meal> Meals { get; set; }
}