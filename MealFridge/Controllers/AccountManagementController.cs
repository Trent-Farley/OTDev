using MealFridge.Models;
using MealFridge.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRestrictionRepo _restrictionContext;
        private readonly IIngredientRepo _ingredientContext;
        private readonly ISavedrecipeRepo _savedRecipeContext;
        private readonly UserManager<IdentityUser> _user;

        public AccountManagementController(IConfiguration config, IRestrictionRepo restrictionContext, IIngredientRepo ingredientContext, ISavedrecipeRepo savedRecipeContext, UserManager<IdentityUser> user)
        {
            _configuration = config;
            _restrictionContext = restrictionContext;
            _ingredientContext = ingredientContext;
            _savedRecipeContext = savedRecipeContext;
            _user = user;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> RemoveRestrictedIngredient(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var temp = _restrictionContext.Restriction(userId, id);
                if (temp != null)
                {
                    _restrictionContext.RemoveRestriction(temp);
                }
            }
            return await Task.FromResult(RedirectToAction("DietaryRestrictions"));
        }
        public async Task<IActionResult> DietaryRestrictions()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var userRestrictions = _restrictionContext.GetUserRestrictedIngred(userId);
                foreach (var restriction in userRestrictions)
                {
                    restriction.Ingred = await _ingredientContext.FindByIdAsync(restriction.IngredId);
                }
                return await Task.FromResult(View("DietaryRestrictions", userRestrictions));
            }
            else
                return await Task.FromResult(RedirectToAction("Index", "Home"));
        }
        public async Task<IActionResult> FoodPreferences()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var userRestrictions = _restrictionContext.GetUserDislikedIngred(userId);
                foreach (var restriction in userRestrictions)
                {
                    restriction.Ingred = await _ingredientContext.FindByIdAsync(restriction.IngredId);
                }
                return await Task.FromResult(View("FoodPreferences", userRestrictions));
            }
            else
                return await Task.FromResult(RedirectToAction("Index", "Home"));
        }
        public async Task<ActionResult> FavoriteRecipes()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var userSavedRecipes = _savedRecipeContext.GetAllRecipes(userId);
                return await Task.FromResult(View("FavoriteRecipes", userSavedRecipes));
            }
            else
                return await Task.FromResult(RedirectToAction("Index", "Home"));
        }

        public async Task<ActionResult> DeleteRecipe(int recipe_id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var temp = _savedRecipeContext.Savedrecipe(userId, recipe_id);
                if(temp != null)
                {
                    _savedRecipeContext.RemoveSavedRecipe(temp);
                }
                
            }
            return await Task.FromResult(RedirectToAction("FavoriteRecipes"));
        }
    }


}

    