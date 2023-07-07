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
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddMeal(Meal meal, CancellationToken cancellationToken)
    {
        _context.Add(meal);
        await _context.SaveChangesAsync(cancellationToken);
    }
}