using CookWithMe.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CookWithMe.Data.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Meal> Meals => Set<Meal>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Meal>()
            .HasOne(x => x.User)
            .WithMany(x => x.Meals)
            .HasForeignKey(x => x.UserId);

        base.OnModelCreating(builder);
    }
}