using MealFridge.Models;
using MealFridge.Models.Interfaces;
using MealFridge.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MealFridge.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IFridgeRepo fridgeRepo;
        private readonly IIngredientRepo ingredientRepo;
        private readonly IRecipeIngredRepo recipeIngredRepo;
        private readonly IRecipeRepo recipeRepo;
        private readonly UserManager<IdentityUser> _user;

        public ShoppingController(IConfiguration config, IFridgeRepo fridge, IIngredientRepo ingRepo, IRecipeIngredRepo resIngredRepo,IRecipeRepo resRepo ,UserManager<IdentityUser> user)
        {
            _configuration = config;
            fridgeRepo = fridge;
            ingredientRepo = ingRepo;
            recipeIngredRepo = resIngredRepo;
            recipeRepo = resRepo;
            _user = user;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var userInventory = fridgeRepo.FindByAccount(userId).Where(i => i.Shopping == true);
                foreach (var ingredient in userInventory)
                    ingredient.Ingred = await ingredientRepo.FindByIdAsync(ingredient.IngredId);
                return await Task.FromResult(View("Index", userInventory.ToList()));
            }
            else
                return await Task.FromResult(RedirectToAction("Index", "Home"));
        }
        /// <summary>
        /// Turns a string of ids seperated by commas into a list of ints
        /// </summary>
        /// <param name="ids">Comma seperated list of ids</param>
        /// <returns>list of ids as ints</returns>
        [NonAction]
        private List<int> MakeIds(string ids) 
        {
            var list = new List<int>();
            var vals = ids.Split(',');
            foreach (var v in vals)
            {
                if (int.TryParse(v, out int x))
                {
                    list.Add(x);
                }
                else
                    Console.Error.WriteLine(x + " is not a number");
            }
            return list;
        }

        [HttpPost]
        public async Task<IActionResult> Obtained(string ids, bool obtain)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = MakeIds(ids);
            foreach (var l in list)
            {
                await fridgeRepo.AddObtained(await fridgeRepo.FindByIdAsync(userId, l), obtain);
            }
            var userInventory = fridgeRepo.FindByAccount(userId);
            foreach (var i in userInventory)
            {
                i.Ingred = await ingredientRepo.FindByIdAsync(i.IngredId);
            }
            //Return the current inventory
            return PartialView("ShoppingList", userInventory);
        }

        //[HttpPost]
        //public async Task<IActionResult> RemoveObtained(string ids)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var list = MakeIds(ids);
        //    foreach (var l in list)
        //    {
        //        await fridgeRepo.DeleteAsync(await fridgeRepo.FindByIdAsync(userId, l));
        //    }
        //    var userInventory = fridgeRepo.FindByAccount(userId);
        //    foreach (var i in userInventory)
        //    {
        //        i.Ingred = await ingredientRepo.FindByIdAsync(i.IngredId);
        //    }
        //    //Return the current inventory
        //    return PartialView("ShoppingList", userInventory);
        //}

        [HttpPost]
        public async Task<IActionResult> AddFridgeItem(int id, int amount)
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
            return PartialView("ShoppingList", userInventory);
        }
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
            fridgeIngredient.NeededAmount += amount;
            //Interact with the repo
            await fridgeRepo.AddFridgeAsync(fridgeIngredient);
            //Get updated inventory
            var userInventory = fridgeRepo.FindByAccount(userId);
            foreach (var i in userInventory)
            {
                i.Ingred = await ingredientRepo.FindByIdAsync(i.IngredId);
            }
            //Return the updated inventory
            return PartialView("ShoppingList", userInventory);
        }
        [HttpPost]
        public async Task<IActionResult> AddRecipeIngredients(string id)
        {
            if (!int.TryParse(id, out var recipeId) || !User.Identity.IsAuthenticated)
                return await Task.FromResult(StatusCode(400));
            var userId = _user.GetUserId(User);
            var recipeIngreds = recipeIngredRepo.GetIngredients(recipeId);
            foreach (var r in recipeIngreds)
            {
                await fridgeRepo.AddRecipeIngred(userId, r);
            }
            return await Task.FromResult(StatusCode(201));
        }
        [HttpPost]
        public async Task AddFromMealPlan(List<string> values)
        {
            foreach (var recipe in values)
            {
                if (recipeRepo.ExistsAsync(Convert.ToInt32(recipe)).Result)
                {
                    await AddRecipeIngredients(recipe);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Swap()
        {
            //Get userid
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Interact with repo
            await fridgeRepo.Swap(userId);
            //Get updated inventory
            var userInventory = fridgeRepo.FindByAccount(userId);
            foreach (var i in userInventory)
            {
                i.Ingred = await ingredientRepo.FindByIdAsync(i.IngredId);
            }
            //Return the updated inventory
            return PartialView("ShoppingList", userInventory);
            
        }
    }
}
