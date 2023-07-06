using CookWithMe.Contracts;
using CookWithMe.Data.Entities;
using CookWithMe.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookWithMe.Controllers;

[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;

    public UserController(UserManager<User> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest registerUserRequest)
    {
        var user = await _userManager.FindByEmailAsync(registerUserRequest.Email);

        if (user is not null)
        {
            // TODO 
            throw new Exception();
        }
        
        // TODO validate data

        var newUser = new User()
        {
            FirstName = registerUserRequest.FirstName,
            LastName = registerUserRequest.LastName,
            UserName = registerUserRequest.Nickname,
            Email = registerUserRequest.Email,
        };
        
        var result = await _userManager.CreateAsync(newUser, registerUserRequest.Password);
        
        return Ok(result);
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
            // TODO
            throw new Exception();
        }
        
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginUserRequest.Password);
        
        if (!isPasswordCorrect)
        {
            // TODO
            throw new Exception();
        }
        
        var token = _jwtService.GenerateToken(user);
        
        return Ok(token);
    }
}