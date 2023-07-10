﻿using CookWithMe.Contracts;
using CookWithMe.Data.Entities;
using CookWithMe.Data.Repositories;
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

    public MealController(IMealRepository mealRepository, UserManager<User> userManager)
    {
        _mealRepository = mealRepository;
        _userManager = userManager;
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
        
        // TODO validate
        
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
        
        if (userId is null || userId != mealToChange.UserId)
        {
            return Unauthorized();
        }
        
        var meal = MapToMeal(updateMealRequest);
        
        // TODO validate

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