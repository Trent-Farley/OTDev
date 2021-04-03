using MealFridge.Models;
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
using MealFridge.Utils;

namespace MealFridge.Controllers
{
    public class FridgeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly MealFridgeDbContext _context;
        private readonly UserManager<IdentityUser> _user;
        private readonly string _ingredientSearchEndpoint = "https://api.spoonacular.com/food/ingredients/search";
        public FridgeController(IConfiguration config, MealFridgeDbContext context, UserManager<IdentityUser> user)
        {
            _configuration = config;
            _context = context;
            _user = user;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var userInventory = _context.Fridges.Where(f => f.AccountId == userId).ToList();
                foreach (var ingredient in userInventory)
                    ingredient.Ingred = _context.Ingredients.Find(ingredient.IngredId);
                return await Task.FromResult(View("Index", userInventory.ToList()));
            }
            else
                return await Task.FromResult(RedirectToAction("Index", "Home"));
        }


        /// <summary>
        /// Main entry point to add an item. This will either udate or add an item to the db
        /// </summary>
        /// <param name="id">Id of the ingredient to be added/updated</param>
        /// <param name="amount">Amount of the ingredient to be added or updated</param>
        /// <returns>A PartialView with the current inventory of the user</returns>
        [HttpPost]
        public async Task<IActionResult> AddItem(int id, int amount)
        {
            //Find the current fridge and user 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fridgeIngredient = _context.Fridges.Where(f => f.Id == id).FirstOrDefault() ?? new Fridge()
            {
                AccountId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                IngredId = id
            };
            fridgeIngredient.Quantity = amount;
            //If this is new, add it to the db
            if (!_context.Fridges.Any(item => item.AccountId == fridgeIngredient.AccountId && item.IngredId == fridgeIngredient.IngredId))
            {
                await _context.Fridges.AddAsync(fridgeIngredient);
                await _context.SaveChangesAsync();
            }
            //else update the fridge with the new amount (Typically +- 1)
            //This may remove the item if there is >1 in the current inventory
            else
                UpdateItem(id, amount);
            //Get the current inventory as it stands with the update/added/removed item
            var userInventory = _context.Fridges.Where(f => f.AccountId == userId)
                .Include(i => i.Ingred)
                .ToList();
            //Return the current inventory
            return PartialView("CurrentInventory", userInventory);
        }

        [HttpPost]
        public async Task<IActionResult> SearchIngredients(Query query)
        {
            var dbIngredients = _context.Ingredients
                .Where(i => i.Name.Contains(query.QueryValue))
                .ToList();
            var possibleIngredients = new List<Ingredient>();

            if (dbIngredients != null)
                possibleIngredients = dbIngredients;

            //We don't need 10 ingredients, 1 is fine. aka, if we type milk, 
            //any milk is fine. If they want to specify 2% milk, then they can search again
            if (possibleIngredients.Count < 1)
            {
                query.QueryName = "query";
                query.Url = _ingredientSearchEndpoint;
                query.Credentials = _configuration["SApiKey"];
                var apiCall = new SearchSpnApi(query);
                possibleIngredients = apiCall.SearchIngredients();
            }
            //Save the ingredients into the db
            foreach (var ingredient in possibleIngredients)
                if (!_context.Ingredients.Any(t => t.Id == ingredient.Id))
                    await _context.AddAsync(ingredient);

            await _context.SaveChangesAsync();


            //Get user inventory to see if they have any of these ingredients
            var userId = _user.GetUserId(User);
            var userInventory = _context.Fridges.Where(f => f.AccountId == userId).ToList();
            foreach (var ingredient in userInventory)
                ingredient.Ingred = _context.Ingredients.Find(ingredient.IngredId);

            //Create a list of ingredients with their current inventory
            var ingredientInventories = new List<IngredientInventory>();
            foreach (var ingredient in possibleIngredients)
            {
                var ingredientInventory = new IngredientInventory()
                {
                    Name = ingredient.Name,
                    Image = ingredient.Image,
                    IngredientId = ingredient.Id
                };
                if (userInventory.Any(f => f.Ingred.Id == ingredient.Id))
                {
                    var fridgeCount = userInventory
                        .Find(f => f.Ingred.Id == ingredient.Id)
                        .Quantity;
                    ingredientInventory.Quantity = fridgeCount ?? 0;
                }
                else
                {
                    ingredientInventory.Quantity = 0;
                }
                ingredientInventories.Add(ingredientInventory);
            }

            return await Task.FromResult(PartialView("IngredientCards", ingredientInventories));
        }

        private void UpdateItem(int id, int amount)
        {
            //Find the ingredient to update, that way not another Fridge is being tracked
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fridgeIngredient = _context.Fridges.Where(f => f.AccountId == userId && f.IngredId == id).FirstOrDefault();
            //set the quantity
            fridgeIngredient.Quantity = amount + (fridgeIngredient.Quantity ?? 0);
            //IF the quantity is 0, remove it
            if (fridgeIngredient.Quantity < 1)
                RemoveItemAsync(id);
            //else update
            else
            {
                _context.Fridges.Update(fridgeIngredient);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Remove a fridge from the database. 
        /// </summary>
        /// <param name="id">The id of an ingredient.</param>
        /// <returns>Void, just removes an item from a db</returns>
        private void RemoveItemAsync(int id)
        {
            //Find the ingredients and corret fridge
            var userId = _user.GetUserId(User);
            var fridgeIngredient = _context.Fridges.Where(f => f.AccountId == userId && f.IngredId == id).FirstOrDefault();
            //IF its there, remove it
            if (_context.Fridges.Any(item => item == fridgeIngredient))
            {
                _context.Remove(fridgeIngredient);
                _context.SaveChanges();
            }
        }

    }

}
