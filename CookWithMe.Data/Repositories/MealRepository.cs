using CookWithMe.Data.Data;
using CookWithMe.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookWithMe.Data.Repositories;

public class MealRepository : IMealRepository
{
    private readonly AppDbContext _context;

    public MealRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Meal?> GetMealById(Guid id, CancellationToken cancellationToken)
        => await _context
            .Meals
            .Where(x => x.DeletedAt == null)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<Meal>> GetUserMeals(Guid id, CancellationToken cancellationToken)
    {
        var meals = await _context
            .Meals
            .Where(x => x.DeletedAt == null && x.UserId == id.ToString())
            .ToListAsync(cancellationToken);

        return meals;
    }

    public async Task AddMeal(Meal meal, CancellationToken cancellationToken)
    {
        _context.Add(meal);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateMeal(Meal meal, CancellationToken cancellationToken)
    {
        _context.Update(meal);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteMeal(Meal meal, CancellationToken cancellationToken)
    {
        meal.ModifiedAt = DateTime.Now;
        meal.DeletedAt = DateTime.Now;

        _context.Update(meal);
        await _context.SaveChangesAsync(cancellationToken);
    }
}