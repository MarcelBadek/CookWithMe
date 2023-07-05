using CookWithMe.Contracts;
using CookWithMe.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookWithMe.Controllers;

[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager)
    {
        _userManager = userManager;
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
}