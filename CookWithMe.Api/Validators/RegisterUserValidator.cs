using CookWithMe.Data.Entities;
using CookWithMe.Helpers;
using FluentValidation;

namespace CookWithMe.Validators;

public class RegisterUserValidator : AbstractValidator<User>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.UserName).NotEmpty().Length(3, 30).Matches(RegexHelper.LettersAndNumbers);
        RuleFor(user => user.Email).NotEmpty().EmailAddress();
        RuleFor(user => user.FirstName).Matches(RegexHelper.Letters);
        RuleFor(user => user.LastName).Matches(RegexHelper.Letters);
    }
}