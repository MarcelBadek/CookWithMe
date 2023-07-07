using CookWithMe.Contracts;
using CookWithMe.Data.Entities;
using CookWithMe.Services;
using CookWithMe.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookWithMe.Controllers;

[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;
    private readonly RegisterUserValidator _userValidator;

    public UserController(UserManager<User> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _userValidator = new RegisterUserValidator();
    }

    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest registerUserRequest)
    {
        var user = MapToUser(registerUserRequest);

        var validationResult = await _userValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        if (await _userManager.FindByEmailAsync(user.Email!) is not null)
        {
            return Conflict("User with this email address already exists");
        }

        if (await _userManager.FindByNameAsync(user.UserName!) is not null)
        {
            return Conflict("User with this nickname address already exists");
        }

        var result = await _userManager.CreateAsync(user, registerUserRequest.Password);
        if (!result.Succeeded)
        {
            return Conflict(result.Errors);
        }

        return Ok("Account created");
    }

    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> Login(LoginUserRequest loginUserRequest)
    {
        User? user = null;

        if (loginUserRequest.Email != "")
        {
            user = await _userManager.FindByEmailAsync(loginUserRequest.Email);
        }

        if (loginUserRequest.Nickname != "")
        {
            user = await _userManager.FindByNameAsync(loginUserRequest.Nickname);
        }

        if (user is null)
        {
            return BadRequest("Invalid data");
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginUserRequest.Password);

        if (!isPasswordCorrect)
        {
            return BadRequest("Invalid data");
        }

        var token = _jwtService.GenerateToken(user);

        return Ok(token);
    }

    private static User MapToUser(RegisterUserRequest registerUserRequest) 
        => new User
    {
        UserName = registerUserRequest.Nickname,
        FirstName = registerUserRequest.FirstName,
        LastName = registerUserRequest.LastName,
        Email = registerUserRequest.Email
    };
}