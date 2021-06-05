using TastyMeals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using TastyMeals.Utils;
using TastyMeals.Models.Interfaces;

namespace TastyMeals.Controllers
{
    public class FridgeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IFridgeRepo fridgeRepo;
        private readonly IIngredientRepo ingredientRepo;
        private readonly IRestrictionRepo restrictionRepo;
        private readonly IRecipeRepo _recipeRepo;
        private readonly ISpnApiService _spnApi;
        private readonly UserManager<IdentityUser> _user;
        private readonly string _ingredientSearchEndpoint = "https://api.spoonacular.com/food/ingredients/search";

        public FridgeController(IConfiguration config, IFridgeRepo fridge, IIngredientRepo ingRepo, IRestrictionRepo resRepo, UserManager<IdentityUser> user, ISpnApiService service, IRecipeRepo recipeRepo, ISavedrecipeRepo savedrecipeRepo)
        {
            _configuration = config;
            fridgeRepo = fridge;
            ingredientRepo = ingRepo;
            restrictionRepo = resRepo;
            _recipeRepo = recipeRepo;
            _user = user;
            _spnApi = service;
        }

        /// <summary>
        /// Main entry into the Fridge View
        /// </summary>
        /// <returns>
        /// If the user is logged in returns their Fridge View, elsewise returns the user to the homepage.
        /// </returns>
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var userInventory = fridgeRepo.FindByAccount(userId);
                foreach (var ingredient in userInventory)
                    ingredient.Ingred = await ingredientRepo.FindByIdAsync(ingredient.IngredId);
                return await Task.FromResult(View("Index", userInventory.ToList()));
            }
            else
                return await Task.FromResult(RedirectToAction("Index", "Home"));
        }

        /// <summary>
        /// Main entry point to add an item. This will either update or add an item to the db
        /// </summary>
        /// <param name="id">Id of the ingredient to be added/updated</param>
        /// <param name="amount">Amount of the ingredient to be added or updated</param>
        /// <returns>A PartialView with the current inventory of the user</returns>
        [HttpPost]
        public async Task<IActionResult> AddItem(int id, int amount)
        {
            //Find the current fridge and user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fridgeIngredient = await fridgeRepo.FindByIdAsync(userId, id) ?? new Fridge()
            {
                AccountId = userId,
                IngredId = id,
                NeededAmount = 0,
                Quantity = 0,
                Shopping = false
            };
            fridgeIngredient.Ingred = await ingredientRepo.FindByIdAsync(id);
            fridgeIngredient.Quantity += amount;
            if (fridgeIngredient.Quantity <= 0 && fridgeIngredient.NeededAmount <= 0)
                RemoveItemAsync(fridgeIngredient);
            if (fridgeIngredient.Quantity < fridgeIngredient.NeededAmount)
                fridgeIngredient.Shopping = true;
            else
                fridgeIngredient.Shopping = false;
            //Add it to the db or update it
            await fridgeRepo.AddFridgeAsync(fridgeIngredient);
            //Get the current inventory as it stands with the update/added/removed item
            var userInventory = fridgeRepo.FindByAccount(userId);
            foreach (var i in userInventory)
            {
                i.Ingred = await ingredientRepo.FindByIdAsync(i.IngredId);
            }
            //Return the current inventory
            return PartialView("CurrentInventory", userInventory);
        }

        [HttpPost]
        public async Task<IActionResult> SearchIngredients(Query query)
        {
            var dbIngredients = ingredientRepo.SearchName(query.QueryValue);

            var possibleIngredients = new List<Ingredient>();

            if (dbIngredients != null)
                possibleIngredients = dbIngredients;

            //We don't need 10 ingredients, 5 is fine. aka, if we type milk,
            //any milk is fine. If they want to specify 2% milk, then they can search again
            bool newIngred = false;
            if (possibleIngredients.Count < 5)
            {
                newIngred = true;
                query.QueryName = "query";
                query.Url = _ingredientSearchEndpoint;
                query.Credentials = _configuration["SApiKey"];
                possibleIngredients = _spnApi.SearchIngredients(query);
            }
            //Save the ingredients into the db
            if (newIngred)
            {
                foreach (var ingredient in possibleIngredients)
                    if (!await ingredientRepo.ExistsAsync(ingredient.Id))
                        await ingredientRepo.AddIngredAsync(ingredient);
            }
            //Get user inventory to see if they have any of these ingredients
            else
            {
                possibleIngredients.RemoveAll(i => restrictionRepo.GetAll().Any(r => r.IngredId == i.Id));
                //await restrictionRepo.RemoveRestrictions(possibleIngredients);
                var userId = _user.GetUserId(User);
                var userInventory = fridgeRepo.FindByAccount(userId);
                foreach (var ingredient in userInventory)
                    ingredient.Ingred = await ingredientRepo.FindByIdAsync(ingredient.IngredId);
            }
            //Create a list of ingredients with their current inventory

            return await Task.FromResult(PartialView("IngredientCards", possibleIngredients));
        }

        /// <summary>
        /// Remove a fridge from the database.
        /// </summary>
        /// <param name="id">The id of an ingredient.</param>
        /// <returns>Void, just removes an item from a db</returns>
        private async void RemoveItemAsync(Fridge fridge)
        {
            if (await fridgeRepo.ExistsAsync(fridge.AccountId, fridge.IngredId))
            {
                await fridgeRepo.DeleteAsync(fridge);
            }
        }

        public async Task<IActionResult> Restriction(int id, string other, Query query)
        {
            var userId = _user.GetUserId(User);
            var badIngred = await ingredientRepo.FindByIdAsync(id);
            var restrict = new Restriction
            {
                Ingred = badIngred,
                AccountId = userId.ToString(),
            };
            if (other == "Banned")
            {
                restrict.Banned = true;
            }
            if (other == "Dislike")
            {
                restrict.Dislike = true;
            }
            await restrictionRepo.AddOrUpdateAsync(restrict);
            return await SearchIngredients(query);
        }
    }
}