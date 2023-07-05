using CookWithMe.Data.Entities;

namespace CookWithMe.Contracts;

public record UpdateMealRequest(
    MealType MealType,
    string Name,
    string Description
);