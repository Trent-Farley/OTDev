using System.Data.Common;
using TastyMeals.Models;
using TastyMeals.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TastyMeals.Models.Interfaces;

namespace TastyMeals.Controllers
{
    public class SearchController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _user;
        private readonly TastyMealsDbContext _db;
        private readonly ISpnApiService _spnApi;
        private readonly IRestrictionRepo _restrictContext;
        private readonly IRecipeIngredRepo _recipeIngredContext;
        private readonly IFridgeRepo _fridgeContext;
        private readonly IRecipeRepo _recipeContext;

        public SearchController(IConfiguration config, TastyMealsDbContext context, UserManager<IdentityUser> user, ISpnApiService service, IRestrictionRepo restrictContext, IFridgeRepo fridgeContext, IRecipeIngredRepo recipeIngredContext, IRecipeRepo recipeRepo)
        {
            _db = context;
            _config = config;
            _user = user;
            _spnApi = service;
            _restrictContext = restrictContext;
            _recipeContext = recipeRepo;
            _recipeIngredContext = recipeIngredContext;
            _fridgeContext = fridgeContext;
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
                    ingredients = ingredients[0..^2];
                return await Task.FromResult(View("Index", ingredients));
            }
            return await Task.FromResult(View());
        }

        [NonAction]
        private bool CheckIntersect(string r, string intersect)
        {
            if (r == null || r == "")
                return false;
            return r.Split(',').Intersect(intersect.Split(',')).Any();
        }

        [HttpPost]
        public async Task<IActionResult> SearchByName(Query query)
        {
            var userId = _user.GetUserId(User);
            var possibleRecipes = _db.Recipes
                .Where(r => r.Title.Contains(query.QueryValue))
                .Include(s => s.Savedrecipes.Where(s => s.AccountId == userId))
                .ToList();
            if (query.CuisineInclude != null)
                possibleRecipes.RemoveAll(r => !CheckIntersect(r.Cuisine, query.CuisineInclude));
            if (query.CuisineExclude != null)
                possibleRecipes.RemoveAll(r => CheckIntersect(r.Cuisine, query.CuisineExclude));
            if (query.Cheap)
            {
                possibleRecipes = possibleRecipes.Where(r => r.Cheap.Value && r.Cost != null).OrderBy(r => r.Cost).ToList();
            }
            else
            {
                possibleRecipes = possibleRecipes.OrderBy(p => p.Id).ToList();
            }
            possibleRecipes = possibleRecipes
                .Skip(10 * query.PageNumber)
                .Take(10)
                .ToList();

            if (possibleRecipes.Count < 10 && userId != null)
            {
                query.Credentials = _config["SApiKey"];
                query.QueryName = "query";
                query.Url = ApiConstants.SearchByNameEndpoint;
                foreach (var i in await SearchApiAsync(query))
                {
                    possibleRecipes.Add(i);
                }
            }
            if (userId != null)
            {
                query = SetDiets(query);
                var banned = _restrictContext.GetUserRestrictedIngredWithIngredName(_restrictContext.GetAll(), userId);
                var dislikes = _restrictContext.GetUserDislikedIngredWithIngredName(_restrictContext.GetAll(), userId);
                foreach (var i in possibleRecipes)
                {
                    var other = _recipeIngredContext.GetIngredients(i.Id);
                    foreach (var j in other)
                    {
                        var temp = _restrictContext.Restriction(_restrictContext.GetAll(), userId, j.IngredId);
                        if (banned.Contains(temp))
                        {
                            i.Banned = true;
                        }
                        if (dislikes.Contains(temp))
                        {
                            i.Dislike = true;
                        }
                    }
                }
            }
            return await Task.FromResult(PartialView("RecipeCards", possibleRecipes.Distinct().Take(10)));
        }

        [HttpPost]
        public async Task<IActionResult> SearchByIngredient(Query query)
        {
            var userId = _user.GetUserId(User);
            var ingredient = _db.Ingredients
                .Where(a => a.Name.Contains(query.QueryValue))
                .FirstOrDefault();//This will be a problem when we are searching for more then one ingredient
            var recipesWithIngredient = _db.Recipeingreds
                .Where(a => a.IngredId == ingredient.Id)
                .Skip(10 * query.PageNumber)
                .Take(10);
            List<Recipe> possibleRecipes = new();
            if (ingredient != null && userId != null)
            {
                foreach (var recipeIngred in recipesWithIngredient)
                {
                    possibleRecipes.Add(_db.Recipes
                        .Where(a => a.Id == recipeIngred.RecipeId)
                        .Include(s => s.Savedrecipes.Where(s => s.AccountId == userId))
                        .FirstOrDefault()
                        );
                }
            }
            if (possibleRecipes.Count < 10)
            {
                query.QueryName = "ingredients";
                query.Url = ApiConstants.SearchByIngredientEndpoint;
                query.Credentials = _config["SApiKey"];
                query.SearchType = "Ingredient";
                foreach (var i in await SearchApiAsync(query))
                {
                    i.Savedrecipes = _db.Savedrecipes.ToList();
                    possibleRecipes.Add(i);
                }
            }
            if (query.Cheap)
            {
                return await Task.FromResult(PartialView("RecipeCards", possibleRecipes.Where(r => r.Cost != null).Distinct().OrderBy(r => r.Cost).Take(10)));
            }
            return await Task.FromResult(PartialView("RecipeCards", possibleRecipes.Distinct().Take(10)));
        }

        public async Task<IActionResult> RecipeDetails(Query query)
        {
            var userId = _user.GetUserId(User);
            var fridge = _db.Fridges.Where(i => i.AccountId == userId).Include(n => n.Ingred).ToList();
            if (!int.TryParse(query.QueryValue, out var id))
                return await Task.FromResult(StatusCode(400));
            var recipe = _db.Recipes
                .Where(rt => rt.Id == id)
                .Include(r => r.Recipeingreds)
                .ThenInclude(i => i.Ingred)
                .FirstOrDefault();
            if (recipe.Recipeingreds.Count < 1)
            {
                query.Credentials = _config["SApiKey"];
                query.QueryName = "id";
                query.Url = ApiConstants.SearchByRecipeEndpoint.Replace("{id}", id.ToString());
                query.SearchType = "Details";
                var recipes = await SearchApiAsync(query);
                recipe.UpdateRecipe(recipes.FirstOrDefault());
                _db.Recipes.Update(recipe);
                _db.SaveChanges();
                _db.ChangeTracker.Clear();
                recipe.Recipeingreds = recipes.FirstOrDefault().Recipeingreds;
                foreach (var ingred in recipe.Recipeingreds)
                {
                    if (!_db.Recipeingreds.Any(r => (r.RecipeId == ingred.RecipeId) && (r.IngredId == ingred.IngredId)))
                    {
                        if (_db.Ingredients.Any(i => i.Id == ingred.IngredId))
                        {
                            ingred.Ingred = _db.Ingredients.FirstOrDefault(i => i.Id == ingred.IngredId);
                        }
                        else
                        {
                            await _db.Ingredients.AddAsync(ingred.Ingred);
                        }
                        await _db.Recipeingreds.AddAsync(ingred);
                        await _db.SaveChangesAsync();
                        _db.ChangeTracker.Clear();
                    }
                }
            }
            var commonIngred = CheckInventory(recipe.Recipeingreds.ToList());
            var viewModel = new FridgeAndRecipeViewModel() { GetFridge = fridge, GetRecipes = recipe, GetIngredients = commonIngred };
            return await Task.FromResult(PartialView("RecipeModal", viewModel));
        }

        /// <summary>
        /// Get new recipes from the api using the SearchSqnApi util. Will return a
        /// list of recipes that have been saved to the db. This is all done asynchronously
        /// </summary>
        /// <param name="query"> A filled out Query with a value and name</param>
        /// <returns>A list of recipes that have been saved to the db</returns>
        private async Task<List<Recipe>> SearchApiAsync(Query query)
        {
            var possibleRecipes = _spnApi.SearchApi(query);
            if (possibleRecipes != null)
                foreach (var recipe in possibleRecipes)
                    if (!_db.Recipes.Any(t => t.Id == recipe.Id))
                        await _db.Recipes.AddAsync(recipe);

            await _db.SaveChangesAsync();
            return await Task.FromResult(possibleRecipes.OrderBy(r => r.Id).Distinct().ToList());
        }

        public async Task<IActionResult> SavedRecipe(int id, string other)
        {
            var userId = _user.GetUserId(User);
            if (userId == null)
                return StatusCode(400);
            var tempRecipe = _db.Recipes.Where(r => r.Id == id).FirstOrDefault();
            var savedRecipe = new SavedRecipe
            {
                Recipe = tempRecipe,
                AccountId = userId.ToString()
            };
            if (other == "Favorite")
                savedRecipe.Favorited = true;

            if (other == "Shelved")
                savedRecipe.Shelved = true;

            var temp = _db.Savedrecipes.ToList();
            foreach (var i in temp)
                if (i.RecipeId == savedRecipe.Recipe.Id)
                    return StatusCode(400);

            await _db.Savedrecipes.AddAsync(savedRecipe);
            await _db.SaveChangesAsync();
            return StatusCode(200);
        }

        public List<Ingredient> CheckInventory(List<RecipeIngredient> recipes)
        {
            var userId = _user.GetUserId(User);
            var fridge = _db.Fridges.Where(i => i.AccountId == userId).Include(n => n.Ingred).ToList();
            var commonIngredients = new List<Ingredient>();

            foreach (var i in fridge)
            {
                foreach (var r in recipes)
                {
                    if (i.Ingred.Id == r.Ingred.Id || i.Ingred.Name == r.Ingred.Name)
                    {
                        commonIngredients.Add(r.Ingred);
                    }
                }
            }

            return commonIngredients;
        }

        public async Task<IActionResult> EmptyInventory(string list, int amount)
        {
            var userId = _user.GetUserId(User);

            foreach (var i in list.Split(' '))
            {
                if (i == "")
                {
                    return await Task.FromResult(StatusCode(200));
                }
                var id = int.Parse(i);
                var fridgeIngredient = _db.Fridges.Where(i => i.AccountId == userId).FirstOrDefault() ?? new Fridge()
                {
                    AccountId = userId,
                    IngredId = id,
                    NeededAmount = 0,
                    Quantity = 0,
                    Shopping = false
                };
                fridgeIngredient.Ingred = _db.Fridges.Where(i => i.IngredId == id).FirstOrDefault().Ingred;
                fridgeIngredient.Quantity += amount;
                if (fridgeIngredient.Quantity <= 0)
                {
                    _db.Fridges.Remove(fridgeIngredient);
                    _db.SaveChanges();
                }
                else
                {
                    var temp = _db.Fridges.First(f => f.AccountId == userId && f.IngredId == fridgeIngredient.IngredId);
                    temp.Quantity += amount;
                    _db.Fridges.Update(temp);
                    _db.SaveChanges();
                }
            }

            //_db.Fridges.Add(fridgeIngredient);
            return await Task.FromResult(StatusCode(200));
        }

        public Query SetDiets(Query query)
        {
            var userId = _user.GetUserId(User);
            var diets = _db.Diets.Where(a => a.AccountId == userId);
            if (!diets.Any())
                return query;
            if (diets.FirstOrDefault().DairyFree == true)
            {
                query.Intolerances = true;
                query.DietInclude += "dairy-free";
            }
            if (diets.FirstOrDefault().Vegan == true)
            {
                query.Diet = true;
                query.DietInclude += "vegan,";
            }
            if (diets.FirstOrDefault().Vegetarian == true)
            {
                query.Diet = true;
                query.DietInclude += "vegetarian";
            }
            if (diets.FirstOrDefault().Keto == true)
            {
                query.Diet = true;
                query.DietInclude += "ketogenic";
            }
            if (diets.FirstOrDefault().GlutenFree == true)
            {
                query.Diet = true;
                query.DietInclude += "gluten-free";
            }
            if (diets.FirstOrDefault().OvoVeg == true)
            {
                query.Diet = true;
                query.DietInclude += "ovo-vegetarian";
            }
            if (diets.FirstOrDefault().LactoVeg == true)
            {
                query.Diet = true;
                query.DietInclude += "lacto-vegetarian";
            }
            if (diets.FirstOrDefault().Pescetarian == true)
            {
                query.Diet = true;
                query.DietInclude += "pescetarian";
            }
            if (diets.FirstOrDefault().Paleo == true)
            {
                query.Diet = true;
                query.DietInclude += "pescetarian";
            }
            if (diets.FirstOrDefault().Primal == true)
            {
                query.Diet = true;
                query.DietInclude += "primal";
            }
            if (diets.FirstOrDefault().Whole30 == true)
            {
                query.Diet = true;
                query.DietInclude += "whole30";
            }
            query.DietInclude = query.DietInclude.Trim(',');
            return query;
        }
    }
}