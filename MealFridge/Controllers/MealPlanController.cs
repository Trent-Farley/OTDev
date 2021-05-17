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

        public MealPlanController(IRecipeRepo ctx, UserManager<IdentityUser> user, ISavedrecipeRepo savedrecipe, IMealRepo mealRepo, IRestrictionRepo resRepo)
        {
            _recipeRepo = ctx;
            _user = user;
            _savedRepo = savedrecipe;
            _mealRepo = mealRepo;
            _restrictionRepo = resRepo;
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

            var meals = new Meals
            {
                Breakfast = breakfast.OrderBy(d => d.Day).ToList(),
                Lunch = Lunch.OrderBy(d => d.Day).ToList(),
                Dinner = dinner.OrderBy(d => d.Day).ToList(),
                DaysCount = breakfast.Count() > 0 ? breakfast.Count() : Lunch.Count() > 0 ? Lunch.Count() : dinner.Count()
            };
            var temp = new MealRestrictions
            {
                Meals = meals,
                MealFilter = new MealFilter()
            };
            return await Task.FromResult(View("Index", temp));
        }

        [HttpPost]
        public async Task<IActionResult> MealPlanFiltered(MealFilter filter)
        {
            if (ModelState.IsValid)
            {
                var userId = _user.GetUserId(User);
                var banned = _restrictionRepo.GetUserRestrictedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
                var dislikes = _restrictionRepo.GetUserDislikedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
                var cDay = DateTime.Now;
                var meals = new Meals();
                meals.DaysCount = filter.Days;
                _mealRepo.RemoveOldMeals();
                if (filter.Breakfast)
                {
                    cDay = cDay.Date + TimeSpan.FromHours(8);
                    meals.Breakfast = _mealRepo.GetMeals(cDay, userId, banned, dislikes, filter.Days, true, filter)
                        .OrderBy(d => d.Day)
                        .ToList();
                }
                if (filter.Lunch)
                {
                    cDay = cDay.Date + TimeSpan.FromHours(12);
                    meals.Lunch = _mealRepo.GetMeals(cDay, userId, banned, dislikes, filter.Days, true, filter)
                        .OrderBy(d => d.Day)
                        .ToList();
                }
                if (filter.Dinner)
                {
                    cDay = cDay.Date + TimeSpan.FromHours(17);
                    meals.Dinner = _mealRepo.GetMeals(cDay, userId, banned, dislikes, filter.Days, true, filter)
                        .OrderBy(d => d.Day)
                        .ToList();
                }

                var temp = new MealRestrictions
                {
                    MealFilter = new MealFilter(),
                    Meals = meals
                };
                return await Task.FromResult(RedirectToAction("Index"));
            }

            return await Task.FromResult(View("Index", new MealRestrictions { MealFilter = filter }));
        }

        [HttpPost]
        public async Task<IActionResult> MealPlan(int days)
        {
            var userId = _user.GetUserId(User);
            var banned = _restrictionRepo.GetUserRestrictedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            var dislikes = _restrictionRepo.GetUserDislikedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            var cDay = DateTime.Now;
            var meals = new Meals();
            meals.DaysCount = days;
            _mealRepo.RemoveOldMeals();
            cDay = cDay.Date + TimeSpan.FromHours(8);
            meals.Breakfast = _mealRepo.GetMeals(cDay, userId, banned, dislikes, days, true)
                .OrderBy(d => d.Day)
                .ToList();

            cDay = cDay.Date + TimeSpan.FromHours(12);
            meals.Lunch = _mealRepo.GetMeals(cDay, userId, banned, dislikes, days, true)
                .OrderBy(d => d.Day)
                .ToList();

            cDay = cDay.Date + TimeSpan.FromHours(17);
            meals.Dinner = _mealRepo.GetMeals(cDay, userId, banned, dislikes, days, true)
                .OrderBy(d => d.Day)
                .ToList();

            if (meals.Breakfast.Count < days || meals.Lunch.Count < days || meals.Dinner.Count < days)
                return await Task.FromResult(PartialView("MealPlan", null));

            return await Task.FromResult(PartialView("MealPlan", meals));
        }

        [HttpPost]
        public async Task<IActionResult> GetFavoritses()
        {
            var user = _user.GetUserId(User);

            return await Task.FromResult(PartialView("SavedRecipesModal", _savedRepo.GetFavoritedRecipe(user)));
        }

        [HttpPost]
        public async Task<IActionResult> MealDetails(Query query)
        {
            if (!int.TryParse(query.QueryValue, out var id))
                return await Task.FromResult(StatusCode(400));
            var meal = _mealRepo.GetAllMealsWithRecipes()
                .First(rt => rt.RecipeId == id);

            return await Task.FromResult(PartialView("MealModal", meal));
        }

        [HttpPost]
        public async Task<IActionResult> RegenerateMeal(string mealDay)
        {
            var swapDay = DateTime.Parse(mealDay);
            var userId = _user.GetUserId(User);
            var banned = _restrictionRepo.GetUserRestrictedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            var dislikes = _restrictionRepo.GetUserDislikedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            var newMeal = _mealRepo.GetMeal(swapDay, userId, banned, dislikes);
            var cDay = DateTime.Now;
            cDay = cDay.Date + TimeSpan.FromHours(8);
            var breakfast = _mealRepo.GetMeals(cDay, userId, banned, dislikes);
            cDay = cDay.Date + TimeSpan.FromHours(12);
            var Lunch = _mealRepo.GetMeals(cDay, userId, banned, dislikes);
            cDay = cDay.Date + TimeSpan.FromHours(17);
            var dinner = _mealRepo.GetMeals(cDay, userId, banned, dislikes);
            var meals = new Meals
            {
                Breakfast = breakfast.OrderBy(d => d.Day).ToList(),
                Lunch = Lunch.OrderBy(d => d.Day).ToList(),
                Dinner = dinner.OrderBy(d => d.Day).ToList(),
                DaysCount = breakfast.Count() > 0 ? breakfast.Count() : Lunch.Count() > 0 ? Lunch.Count() : dinner.Count()
            };
            return await Task.FromResult(PartialView("MealPlan", meals));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOldMealPlan()
        {
            _mealRepo.RemoveOldMeals();
            return await Task.FromResult(RedirectToAction("Index"));
        }

        public async Task<IActionResult> SavedRecipe(int id, string other)
        {
            var userId = _user.GetUserId(User);
            var favRecipe = await _recipeRepo.FindByIdAsync(id);
            var recipe = new Savedrecipe
            {
                Recipe = favRecipe,
                AccountId = userId.ToString(),
            };
            if (other == "Shelved")
            {
                recipe.Favorited = false;
                recipe.Shelved = true;
            }
            if (other == "Favorite")
            {
                recipe.Favorited = true;
                recipe.Shelved = false;
            }
            if (!_savedRepo.GetFavoritedRecipe(userId).Contains(recipe))
            {
                await _savedRepo.AddOrUpdateAsync(recipe);
            }
            return StatusCode(200);
        }
    }
}