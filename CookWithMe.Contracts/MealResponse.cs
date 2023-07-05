using CookWithMe.Data.Entities;

namespace CookWithMe.Contracts;

public record MealResponse(
    Guid Id,
    MealType MealType,
    DateTime CreatedDateTime,
    DateTime ModifiedDateTime,
    string Name,
    string Description,
    string AuthorName
);