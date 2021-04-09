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

namespace MealFridge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MealFridgeDbContext _db;
        public HomeController(ILogger<HomeController> logger, MealFridgeDbContext context)
        {
            _logger = logger;
            _db = context;
        }

        public async Task<IActionResult> Index()
        {
            var randomRecipes = _db.Recipes
                .OrderBy(r => Guid.NewGuid())
                .Take(6)
                .ToList();
            return await Task.FromResult(View("Index", randomRecipes));
        }
        [HttpPost]
        public async Task<IActionResult> RecipeDetails(Query query)
        {
            return await Task.FromResult(RedirectToAction("RecipeDetails", "Search", new { QueryValue = query.QueryValue }));
        }
    }
}
