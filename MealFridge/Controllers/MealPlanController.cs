using MealFridge.Models;
using MealFridge.Models.Interfaces;
using MealFridge.Models.ViewModels;
using MealFridge.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Controllers
{
    public class MealPlanController : Controller
    {
        private IRecipeRepo _recipeRepo;
        private UserManager<IdentityUser> _user;
        private ISavedrecipeRepo _savedRepo;
        private IMealRepo _mealRepo;
        private IRestrictionRepo _restrictionRepo;
        private ISavedrecipeRepo _savedRecipeRepo;

        public MealPlanController(IRecipeRepo ctx, UserManager<IdentityUser> user, ISavedrecipeRepo savedrecipe, IMealRepo mealRepo, IRestrictionRepo resRepo, ISavedrecipeRepo savedRepo)
        {
            _recipeRepo = ctx;
            _user = user;
            _savedRepo = savedrecipe;
            _mealRepo = mealRepo;
            _restrictionRepo = resRepo;
            _savedRecipeRepo = savedRepo;
        }

        public async Task<IActionResult> Index() =>
            await Task.FromResult(View());

        [HttpPost]
        public async Task<IActionResult> MealPlan(int days)
        {
            var userId = _user.GetUserId(User);
            var banned = _restrictionRepo.GetUserRestrictedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            var dislikes = _restrictionRepo.GetUserDislikedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            Console.WriteLine(_mealRepo.GetMeal("Breakfast", userId, banned, dislikes));
            var meals = new Meals
            {
                Breakfast = Meal.CreateMealsFromRecipes(_recipeRepo.GetAll()
                    .Where(r => r.Breakfast == true)
                    .OrderBy(r => Guid.NewGuid())
                    .Take(days)
                    .ToList()),
                Lunch = Meal.CreateMealsFromRecipes(_recipeRepo.GetAll()
                   .Where(r => r.Lunch == true)
                   .OrderBy(r => Guid.NewGuid())
                   .Take(days)
                   .ToList()),
                Dinner = Meal.CreateMealsFromRecipes(_recipeRepo.GetAll()
                   .Where(r => r.Dinner == true)
                   .OrderBy(r => Guid.NewGuid())
                   .Take(days)
                   .ToList()),
                Days = DatesGenerator.GetDays(days)
            };
            return await Task.FromResult(PartialView("MealPlan", meals));
        }

        [HttpPost]
        public async Task<IActionResult> GetFavoritses()
        {
            var user = _user.GetUserId(User);

            return await Task.FromResult(PartialView("SavedRecipesModal", _savedRepo.GetFavoritedRecipe(user)));
        }

        [HttpPost]
        public async Task<IActionResult> RecipeDetails(Query query)
        {
            return await Task.FromResult(RedirectToAction("RecipeDetails", "Search", new { query.QueryValue }));
        }

        //[HttpPost]
        //public async Task<IActionResult> RecipeDetails(Query query)
        //{
        //    return await Task.FromResult(RedirectToAction("RecipeDetails", "Search", new { query.QueryValue }));
        //}

        public async Task<IActionResult> SavedRecipe(int id, string other)
        {
            var userId = _user.GetUserId(User);
            var favRecipe = await _recipeRepo.FindByIdAsync(id);
            var recipe = new Savedrecipe
            {
                Recipe = favRecipe,
                AccountId = userId.ToString(),
            };
            if(other == "Shelved")
            {
                recipe.Favorited = false;
                recipe.Shelved = true;
            }
            if (other == "Favorite")
            {
                recipe.Favorited = true;
                recipe.Shelved = false;
            }
            if (!_savedRecipeRepo.GetFavoritedRecipe(userId).Contains(recipe))
            {
                await _savedRecipeRepo.AddOrUpdateAsync(recipe);
            }
            return StatusCode(200);
        }

    } 
}