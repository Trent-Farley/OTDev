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

        public async Task<IActionResult> Index()
        {
            var userId = _user.GetUserId(User);
            var banned = _restrictionRepo.GetUserRestrictedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            var dislikes = _restrictionRepo.GetUserDislikedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            var cDay = DateTime.Now;
            cDay = cDay.Date + TimeSpan.FromHours(8);
            var breakfast = _mealRepo.GetMeals(cDay, userId, banned, dislikes);
            cDay = cDay.Date + TimeSpan.FromHours(12);
            var Lunch = _mealRepo.GetMeals(cDay, userId, banned, dislikes);
            cDay = cDay.Date + TimeSpan.FromHours(17);
            var dinner = _mealRepo.GetMeals(cDay, userId, banned, dislikes);
            if (breakfast.Count() < 1 || Lunch.Count() < 1 || dinner.Count() < 1)
                return await Task.FromResult(View("Index", null));

            var meals = new Meals
            {
                Breakfast = breakfast.OrderBy(d => d.Day).ToList(),
                Lunch = Lunch.OrderBy(d => d.Day).ToList(),
                Dinner = dinner.OrderBy(d => d.Day).ToList(),
            };
            return await Task.FromResult(View("Index", meals));
        }

        [HttpPost]
        public async Task<IActionResult> MealPlan(int days)
        {
            var userId = _user.GetUserId(User);
            var banned = _restrictionRepo.GetUserRestrictedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            var dislikes = _restrictionRepo.GetUserDislikedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            var cDay = DateTime.Now;
            cDay = cDay.Date + TimeSpan.FromHours(8);
            var breakfast = _mealRepo.GetMeals(cDay, userId, banned, dislikes, days, true);
            cDay = cDay.Date + TimeSpan.FromHours(12);
            var Lunch = _mealRepo.GetMeals(cDay, userId, banned, dislikes, days, true);
            cDay = cDay.Date + TimeSpan.FromHours(17);
            var dinner = _mealRepo.GetMeals(cDay, userId, banned, dislikes, days, true);
            if (breakfast.Count < days || Lunch.Count < days || dinner.Count < days)
                return await Task.FromResult(PartialView("MealPlan", null));

            var meals = new Meals
            {
                Breakfast = breakfast,
                Lunch = Lunch,
                Dinner = dinner,
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
            if (!int.TryParse(query.QueryValue, out var id))
                return await Task.FromResult(StatusCode(400));
            var meal = _mealRepo.GetAll()
                .Where(rt => rt.RecipeId == id)
                .FirstOrDefault();
            return await Task.FromResult(PartialView("MealModal", meal));
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