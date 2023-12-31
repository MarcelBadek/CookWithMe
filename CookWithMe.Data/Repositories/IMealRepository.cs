﻿using CookWithMe.Data.Entities;

namespace CookWithMe.Data.Repositories;

public interface IMealRepository
{
    Task<Meal?> GetMealById(Guid id, CancellationToken cancellationToken);
    Task<List<Meal>> GetUserMeals(Guid id, CancellationToken cancellationToken);
    Task AddMeal(Meal meal, CancellationToken cancellationToken);
    Task UpdateMeal(Meal meal, CancellationToken cancellationToken);
    Task DeleteMeal(Meal meal, CancellationToken cancellationToken);
}