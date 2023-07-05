namespace CookWithMe.Contracts;

public record RegisterUserRequest(
    string Nickname,
    string FirstName,
    string LastName,
    string Email,
    string Password
);