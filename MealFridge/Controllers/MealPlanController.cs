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
        private IRecipeRepo _recipeDb;
        private UserManager<IdentityUser> _user;

        public MealPlanController(IRecipeRepo ctx, UserManager<IdentityUser> user)
        {
            _recipeDb = ctx;
            _user = user;
        }

        public async Task<IActionResult> Index() =>
            await Task.FromResult(View());

        [HttpPost]
        public async Task<IActionResult> MealPlan(int days)
        {
            var meals = new Meals
            {
                Breakfast = Meal.CreateMealsFromRecipes(_recipeDb.GetAll()
               .Where(r => r.Breakfast == true)
               .OrderBy(r => Guid.NewGuid())
               .Take(days)
               .ToList()),
                Lunch = Meal.CreateMealsFromRecipes(_recipeDb.GetAll()
               .Where(r => r.Lunch == true)
               .OrderBy(r => Guid.NewGuid())
               .Take(days)
               .ToList()),
                Dinner = Meal.CreateMealsFromRecipes(_recipeDb.GetAll()
               .Where(r => r.Dinner == true)
               .OrderBy(r => Guid.NewGuid())
               .Take(days)
               .ToList()),
                Days = DatesGenerator.GetDays(days)
            };
            return await Task.FromResult(PartialView("MealPlan", meals));
        }

        public async Task<IActionResult> GetFavorties()
        {
            return await Task.FromResult(PartialView("SavedRecipes"));
        }
    }
}