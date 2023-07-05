using CookWithMe.Data.Entities;

namespace CookWithMe.Contracts;

public record CreateMealRequest(
    MealType MealType,
    string Name,
    string Description
);