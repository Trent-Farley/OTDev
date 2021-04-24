using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MealFridge.Models;
using MealFridge.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using MealFridge.Utils;



namespace MealFridge.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly IConfiguration _configuration;
        //private readonly IFridgeRepo fridgeRepo;
        //private readonly IIngredientRepo ingredientRepo;
        //private readonly IRecipeIngredRepo recipeRepo;
        private readonly ISavedrecipeRepo savedrecipeRepo;
        private readonly UserManager<IdentityUser> _user;
        private readonly MealFridgeDbContext _db;

        public AccountManagementController(IConfiguration config, UserManager<IdentityUser> user, ISavedrecipeRepo other)
        {
            _configuration = config;
           
            //fridgeRepo = fridge;
            //ingredientRepo = ingRepo;
            //recipeRepo = resRepo;
            savedrecipeRepo = other;
            _user = user;
        }

        public ActionResult  Index()
        {
            return View();
        }
        public ActionResult DietaryRestrictions()
        {
            return View();
        }
        public ActionResult FoodPreferences()
        {
            return View();
        }
        public async Task<ActionResult> FavoriteRecipes()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
               // var userSavedRecipes = _db.Savedrecipes.Where(f => f.AccountId == userId).Include(r => r.Recipe).ToList();
                var userSavedRecipes = savedrecipeRepo.FindAccount(userId);
                return await Task.FromResult(View("FavoriteRecipes", userSavedRecipes.ToList()));
            }
            return await Task.FromResult(RedirectToAction("Index", "Home"));
        }

       public async Task<ActionResult> DeleteRecipe(int recipe_id)
        {
            var userId = _user.GetUserId(User);
            var temp = savedrecipeRepo.FindAccount(userId);
            foreach(var i in temp)
            {
                if (i.RecipeId == recipe_id)
                {
                    savedrecipeRepo.RemoveSavedRecipe(i);
                }
            }
          
            return await Task.FromResult(RedirectToAction("FavoriteRecipes"));
        }
   }
}