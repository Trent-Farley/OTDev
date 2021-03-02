using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MealFridge.Utils;
using System;
using MealFridge.Models;
using System.Linq;
namespace MealFridge.Controllers
{
    public class SearchController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _searchByNameEndpoint = "https://api.spoonacular.com/recipes/complexSearch";
        private readonly MealFridgeDbContext _db;
        public SearchController(IConfiguration config, MealFridgeDbContext context)
        {
            _db = context;
            _config = config;
        }

        [Route("api/SearchByName/{query}")]
        public IActionResult SearchByName(string query)
        {
            var possibleRecipes = _db.Recipes
                .Where(r => r.Title.Contains(query))
                .OrderBy(p => p.Id)
                .Take(10)
                .ToList();
            if (possibleRecipes.Count < 10)
            {
                var apiQuerier = new SearchSpnApi(_searchByNameEndpoint, _config["SApiKey"]);
                possibleRecipes = apiQuerier.SearchAPI(query);
                foreach (var recipe in possibleRecipes)
                {
                    if (!_db.Recipes.Any(t => t.Id == recipe.Id))
                    {
                        _db.Recipes.Add(recipe);
                    }
                }
                _db.SaveChanges();
            }

            return Json(possibleRecipes.OrderBy(r => r.Id).ToList());
        }
    }
}
