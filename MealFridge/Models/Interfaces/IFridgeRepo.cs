﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Models.Interfaces
{
    public interface IFridgeRepo : IRepository<Fridge>
    {
        Task<bool> ExistsAsync(string userId, int ingredId);        
        Task<Fridge> FindByIdAsync(string userId, int ingredId);
        List<Fridge> FindByAccount(string userId);
        List<Ingredient> FindByIngredient(int ingredId);
        Task AddFridgeAsync(Fridge fridgeIngredient);
        Task AddRecipeIngred(string userId, RecipeIngredient r);
        Task AddObtained(Fridge item, bool obtained);
        Task Swap(string userId);
    }
}
