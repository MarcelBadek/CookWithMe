namespace CookWithMe.Contracts;

public record LoginUserRequest(
    string Email,
    string Nickname,
    string Password
    );