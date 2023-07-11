using CookWithMe.Contracts;
using CookWithMe.Data.Entities;
using CookWithMe.Data.Repositories;
using CookWithMe.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookWithMe.Controllers;

[ApiController]
[Route("/meal")]
public class MealController : ControllerBase
{
    private readonly IMealRepository _mealRepository;
    private readonly UserManager<User> _userManager;
    private readonly MealValidator _mealValidator;

    public MealController(IMealRepository mealRepository, UserManager<User> userManager)
    {
        _mealRepository = mealRepository;
        _userManager = userManager;
        _mealValidator = new MealValidator();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMeal(Guid id)
    {
        var meal = await _mealRepository.GetMealById(id, new CancellationToken());
        
        if (meal is null)
        {
            return BadRequest($"Meal with id: {id} does not exists");
        }

        var user = await _userManager.FindByIdAsync(meal.UserId);
        var userName = user!.UserName!;

        var response = MapToMealResponse(meal, userName);

        return Ok(response);
    }

    [HttpGet("/user/{id:guid}")]
    public async Task<IActionResult> GetUserMeals(Guid id)
    {
        var meals = await _mealRepository.GetUserMeals(id, new CancellationToken());

        if (meals.Count == 0)
        {
            return BadRequest($"User with id: {id} does not have meals");
        }
        
        var user = await _userManager.FindByIdAsync(id.ToString());
        var userName = user!.UserName!;

        var list = meals.Select(meal => MapToMealResponse(meal, userName)).ToList();

        return Ok(list);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateMeal(CreateMealRequest createMealRequest)
    {
        var userId = await GetUserId();
        
        if (userId is null)
        {
            return Unauthorized();
        }

        var meal = MapToMeal(createMealRequest, userId);

        var validationResult = await _mealValidator.ValidateAsync(meal);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        await _mealRepository.AddMeal(meal, new CancellationToken());
        
        return Ok();
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMeal(Guid id, UpdateMealRequest updateMealRequest)
    {
        var mealToChange = await _mealRepository.GetMealById(id, new CancellationToken());

        if (mealToChange is null)
        {
            return BadRequest($"Meal with id: {id} does not exists");
        }
        
        var userId = await GetUserId();
        
        if (userId != mealToChange.UserId)
        {
            return Unauthorized();
        }
        
        var meal = MapToMeal(updateMealRequest);
        
        var validationResult = await _mealValidator.ValidateAsync(meal);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        mealToChange.MealType = meal.MealType;
        mealToChange.Name = meal.Name;
        mealToChange.Description = meal.Description;
        mealToChange.ModifiedAt = DateTime.Now;

        await _mealRepository.UpdateMeal(mealToChange, new CancellationToken());

        return Ok();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMeal(Guid id)
    {
        var mealToDelete = await _mealRepository.GetMealById(id, new CancellationToken());

        if (mealToDelete is null)
        {
            return BadRequest($"Meal with id: {id} does not exists");
        }

        var userId = await GetUserId();
        
        if (userId != mealToDelete.UserId)
        {
            return Unauthorized();
        }

        await _mealRepository.DeleteMeal(mealToDelete, new CancellationToken());

        return Ok();
    }

    private static Meal MapToMeal(CreateMealRequest createMealRequest, string userId)
        => new Meal
        {
            MealType = createMealRequest.MealType,
            Name = createMealRequest.Name,
            Description = createMealRequest.Description,
            UserId = userId
        };
    
    private static Meal MapToMeal(UpdateMealRequest updateMealRequest)
        => new Meal
        {
            MealType = updateMealRequest.MealType,
            Name = updateMealRequest.Name,
            Description = updateMealRequest.Description
        };
    
    private static MealResponse MapToMealResponse(Meal meal, string userName)
        => new MealResponse
        (
            meal.Id,
            meal.MealType,
            meal.CreatedAt,
            meal.ModifiedAt,
            meal.Name,
            meal.Description,
            userName
        );

    private async Task<string?> GetUserId()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
        
        if (userId is null)
        {
            return null;
        }

        if (await _userManager.FindByIdAsync(userId) is null)
        {
            return null;
        }

        return userId;
    }
}