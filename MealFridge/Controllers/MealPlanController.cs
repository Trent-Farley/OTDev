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

        public async Task<IActionResult> Index() =>
            await Task.FromResult(View());

        [HttpPost]
        public async Task<IActionResult> MealPlan(int days)
        {
            var userId = _user.GetUserId(User);
            var banned = _restrictionRepo.GetUserRestrictedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();
            var dislikes = _restrictionRepo.GetUserDislikedIngredWithIngredName(_restrictionRepo.GetAll(), userId).Select(i => i.Ingred).ToList();

            var breakfast = _mealRepo.GetMeals("Breakfast", userId, banned, dislikes, days);
            var Lunch = _mealRepo.GetMeals("Lunch", userId, banned, dislikes, days);
            var dinner = _mealRepo.GetMeals("Lunch", userId, banned, dislikes, days);
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
            return await Task.FromResult(RedirectToAction("RecipeDetails", "Search", new { query.QueryValue }));
        }
    }
}