using MealFridge.Models;
using MealFridge.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MealFridge.Models.Interfaces;

namespace MealFridge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly MealFridgeDbContext _db;
        private readonly IRecipeRepo _db;
        public HomeController(ILogger<HomeController> logger, IRecipeRepo context)
        {
            _logger = logger;
            _db = context;
        }

        public async Task<IActionResult> Index()
        {
            //Seed recipes with https://spoonacular.com/food-api/docs#Get-Random-Recipes
            var randomRecipes = _db.getRandomSix();
            return await Task.FromResult(View("Index", randomRecipes));
        }
        [HttpPost]
        public async Task<IActionResult> RecipeDetails(Query query)
        {
            return await Task.FromResult(RedirectToAction("RecipeDetails", "Search", new { QueryValue = query.QueryValue }));
        }
    }
}
