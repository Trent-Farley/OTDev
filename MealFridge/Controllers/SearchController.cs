using System.Data.Common;
using MealFridge.Models;
using MealFridge.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Controllers
{
    public class SearchController : Controller
    {

        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _user;
        private readonly MealFridgeDbContext _db;
        private readonly string _searchByNameEndpoint = "https://api.spoonacular.com/recipes/complexSearch";
        private readonly string _searchByIngredientEndpoint = "https://api.spoonacular.com/recipes/findByIngredients";
        private readonly string _searchByRecipeEndpoint = "https://api.spoonacular.com/recipes/{id}/information";
        public SearchController(IConfiguration config, MealFridgeDbContext context, UserManager<IdentityUser> user)
        {
            _db = context;
            _config = config;
            _user = user;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user_id = _user.GetUserId(User);
                var temp = _db.Fridges.Where(f => f.AccountId == user_id);
                string ingredients = "";
                foreach (var f in temp)
                    ingredients += _db.Ingredients.Where(a => a.Id == f.IngredId).FirstOrDefault().Name + ", ";

                if (ingredients.Length > 2)
                    ingredients = ingredients.Substring(0, ingredients.Length - 2);
                return await Task.FromResult(View("Index", ingredients));
            }
            return await Task.FromResult(View());
        }
        [HttpPost]
        public async Task<IActionResult> SearchByName(Query query)
        {
            var possibleRecipes = _db.Recipes
                .Where(r => r.Title.Contains(query.QueryValue))
                .OrderBy(p => p.Id)
                .Take(10)
                .ToList();
            if (possibleRecipes.Count < 10)
            {
                query.Credentials = _config["SApiKey"];
                query.QueryName = "query";
                query.Url = _searchByNameEndpoint;
                possibleRecipes = await SearchApiAsync(query);
            }
            return await Task.FromResult(PartialView("RecipeCards", possibleRecipes));
        }
        [HttpPost]
        public async Task<IActionResult> SearchByIngredient(Query query)
        {
            var ingredient = _db.Ingredients.Where(a => a.Name.Contains(query.QueryValue)).FirstOrDefault();//This will be a problem when we are searching for more then one ingredient
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
                query.QueryName = "ingredients";
                query.Url = _searchByIngredientEndpoint;
                query.Credentials = _config["SApiKey"];
                query.SearchType = "Ingredient";
                possibleRecipes = await SearchApiAsync(query);
            }
            return await Task.FromResult(PartialView("RecipeCards", possibleRecipes));
        }
        [HttpPost]
        public async Task<IActionResult> RecipeDetails(Query query)
        {
            if (!int.TryParse(query.QueryValue, out var id))
                return await Task.FromResult(StatusCode(400));
            var recipe = _db.Recipes
                .Where(rt => rt.Id == id)
                .FirstOrDefault();
            if (recipe.Recipeingreds.Count < 1)
            {
                query.Credentials = _config["SApiKey"];
                query.QueryName = "id";
                query.Url = _searchByRecipeEndpoint.Replace("{id}", id.ToString());
                query.SearchType = "Details";
                var recipes = await SearchApiAsync(query);
                recipe = recipes.FirstOrDefault();
            }
            return await Task.FromResult(PartialView("RecipeModal", recipe));
        }
        /// <summary>
        /// Get new recipes from the api using the SearchSqnApi util. Will return a 
        /// list of recipes that have been saved to the db. This is all done asynchronously 
        /// </summary>
        /// <param name="query"> A filled out Query with a value and name</param>
        /// <returns>A list of recipes that have been saved to the db</returns>
        private async Task<List<Recipe>> SearchApiAsync(Query query)
        {
            var apiQuerier = new SearchSpnApi(query);
            var possibleRecipes = apiQuerier.SearchAPI();
            foreach (var recipe in possibleRecipes)
                if (!_db.Recipes.Any(t => t.Id == recipe.Id))
                    await _db.Recipes.AddAsync(recipe);

            await _db.SaveChangesAsync();
            return await Task.FromResult(possibleRecipes.OrderBy(r => r.Id).ToList());
        }
    }
}
