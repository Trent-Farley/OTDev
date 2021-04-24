using MealFridge.Models.Interfaces;
using MealFridge.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IFridgeRepo fridgeRepo;
        private readonly IIngredientRepo ingredientRepo;
        private readonly IRecipeIngredRepo recipeRepo;
        private readonly UserManager<IdentityUser> _user;

        public ShoppingController(IConfiguration config, IFridgeRepo fridge, IIngredientRepo ingRepo, IRecipeIngredRepo resRepo, UserManager<IdentityUser> user)
        {
            _configuration = config;
            fridgeRepo = fridge;
            ingredientRepo = ingRepo;
            recipeRepo = resRepo;
            _user = user;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var userInventory = fridgeRepo.FindByAccount(userId).Where(i => i.Shopping == true);
                foreach (var ingredient in userInventory)
                    ingredient.Ingred = await ingredientRepo.FindByIdAsync(ingredient.IngredId);
                return await Task.FromResult(View("Index", userInventory.ToList()));
            }
            else
                return await Task.FromResult(RedirectToAction("Index", "Home"));
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipeIngredients(string id)
        {
            if (!int.TryParse(id, out var recipeId) || !User.Identity.IsAuthenticated)
                return await Task.FromResult(StatusCode(400));
            var userId = _user.GetUserId(User);
            var recipeIngreds = recipeRepo.GetIngredients(recipeId);
            foreach (var r in recipeIngreds)
            {
                if (await fridgeRepo.ExistsAsync(userId, r.IngredId))
                {
                    var item = await fridgeRepo.FindByIdAsync(userId, r.IngredId);
                    item.NeededAmount += r.Amount;
                    if (item.NeededAmount > item.Quantity)
                        item.Shopping = true;
                }
                else
                {
                    await fridgeRepo.AddAsync(new Models.Fridge
                    {
                        AccountId = userId,
                        IngredId = r.IngredId,
                        NeededAmount = r.Amount,
                        Shopping = true
                    });
                }
            }
            return await Task.FromResult(StatusCode(201));
        }
    }
}
