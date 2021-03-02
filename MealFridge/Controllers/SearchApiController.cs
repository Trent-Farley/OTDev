using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MealFridge.Utils;
using System;
using MealFridge.Models;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;


namespace MealFridge.Controllers
{
    public class SearchApiController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _searchByNameEndpoint = "https://api.spoonacular.com/recipes/complexSearch";

        private readonly string _searchByIngredientEndpoint = "https://api.spoonacular.com/recipes/findByIngredients";
        private readonly MealFridgeDbContext _db;
        public SearchApiController(IConfiguration config, MealFridgeDbContext context)
        {
            _db = context;
            _config = config;
        }
        public List<Recipe> SearchByIngredient(string query)
        {
            Debug.WriteLine("Search By Ingredient Query: " + query);
            var ingredient = _db.Ingredients.Where(a => a.Name.Contains(query)).FirstOrDefault();
            var recipesWithIngredient = _db.Recipeingreds.Where(a => a.IngredId == ingredient.Id).Take(10);
            List<Recipe> possibleRecipes = new List<Recipe>();
            
            if (ingredient != null)
            { 
                foreach (var recipeIngred in recipesWithIngredient)
                {
                    possibleRecipes.Add(_db.Recipes.Where(a => a.Id == recipeIngred.RecipeId).FirstOrDefault());
                }
            }

            if (possibleRecipes.Count < 10)
            {
                var apiQuerier = new SearchSpnApi(_searchByIngredientEndpoint, _config["SApiKey"]);
                possibleRecipes = apiQuerier.SearchAPI(query, "Ingredient");
                if (possibleRecipes == null)
                {
                    possibleRecipes = new List<Recipe>();
                }
                foreach (var recipe in possibleRecipes)
                {
                    if (!_db.Recipes.Any(t => t.Id == recipe.Id))
                    {
                        _db.Recipes.Add(recipe);
                    }
                }
                _db.SaveChanges();
            }
            return possibleRecipes;
        }

        [Route("api/SearchByName/{query}/{type}")]
        public IActionResult SearchByName(string query, string type)
        {
            if (type == "Ingredient")
            {
                var possibleRecipesByIngredient = SearchByIngredient(query);
                return Json(possibleRecipesByIngredient.OrderBy(r => r.Id).ToList());
            }
            var possibleRecipes = _db.Recipes
                .Where(r => r.Title.Contains(query))
                .OrderBy(p => p.Id)
                .Take(10)
                .ToList();
            if (possibleRecipes.Count < 10)
            {
                var apiQuerier = new SearchSpnApi(_searchByNameEndpoint, _config["SApiKey"]);
                possibleRecipes = apiQuerier.SearchAPI(query, "Recipe");
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
