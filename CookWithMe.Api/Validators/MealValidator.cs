using CookWithMe.Data.Entities;
using FluentValidation;

namespace CookWithMe.Validators;

public class MealValidator : AbstractValidator<Meal>
{
    public MealValidator()
    {
        RuleFor(meal => meal.MealType).NotEmpty().IsInEnum();
        RuleFor(meal => meal.Name).Length(3, 300);
        RuleFor(meal => meal.Description).Length(3, 10000);
    }    
}