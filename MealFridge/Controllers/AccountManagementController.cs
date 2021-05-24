using MealFridge.Models;
using MealFridge.Models.Interfaces;
using MealFridge.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MealFridge.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRestrictionRepo _restrictionContext;
        private readonly IIngredientRepo _ingredientContext;
        private readonly ISavedrecipeRepo _savedRecipeContext;
        private readonly IDietRepo _dietContext;
        private readonly UserManager<IdentityUser> _user;

        public AccountManagementController(IConfiguration config, IRestrictionRepo restrictionContext, IIngredientRepo ingredientContext, ISavedrecipeRepo savedRecipeContext, IDietRepo dietContext, UserManager<IdentityUser> user)
        {
            _configuration = config;
            _restrictionContext = restrictionContext;
            _ingredientContext = ingredientContext;
            _savedRecipeContext = savedRecipeContext;
            _dietContext = dietContext;
            _user = user;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UpdateDiet(bool whole30, bool dairyFree, bool glutenFree, bool keto, bool vegan, bool vegetarian, bool lactoVeg, bool ovoVeg, bool paleo, bool pescetarian, bool primal, bool metric)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var diet = new Diet()
                {
                    AccountId = userId,
                    Whole30 = whole30,
                    DairyFree = dairyFree,
                    GlutenFree = glutenFree,
                    Keto = keto,
                    Vegan = vegan,
                    Vegetarian = vegetarian,
                    LactoVeg = lactoVeg,
                    OvoVeg = ovoVeg,
                    Paleo = paleo,
                    Pescetarian = pescetarian,
                    Primal = primal,
                    Metric = metric
                };
                await _dietContext.AddOrUpdateAsync(diet);
                return await Task.FromResult(RedirectToAction("DietaryRestrictions"));
            }
            return await Task.FromResult(RedirectToAction("Index", "Home"));
        }

        public async Task<IActionResult> RemoveRestrictedIngredient(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var temp = _restrictionContext.Restriction(_restrictionContext.GetAll(), userId, id);
                if (temp != null)
                {
                    await _restrictionContext.DeleteAsync(temp);
                }
            }
            return await Task.FromResult(RedirectToAction("DietaryRestrictions"));
        }

        public async Task<IActionResult> DietaryRestrictions()
        {
            if (User.Identity.IsAuthenticated)
            {
                var dietRestr = new DietaryRestrictions();
                var userId = _user.GetUserId(User);
                var userRestrictions = _restrictionContext.GetUserRestrictedIngred(_restrictionContext.GetAll(), userId);
                foreach (var restriction in userRestrictions)
                {
                    restriction.Ingred = await _ingredientContext.FindByIdAsync(restriction.IngredId);
                }
                dietRestr.userRestrictions = userRestrictions;
                dietRestr.Diet = await _dietContext.FindByIdAsync(userId);
                if (dietRestr.Diet == null)
                {
                    dietRestr.Diet = new Diet()
                    {
                        GlutenFree = false,
                        Keto = false,
                        DairyFree = false,
                        LactoVeg = false,
                        OvoVeg = false,
                        Paleo = false,
                        Pescetarian = false,
                        Primal = false,
                        Vegan = false,
                        Vegetarian = false,
                        Whole30 = false,
                        Metric = false
                    };
                }
                return await Task.FromResult(View("DietaryRestrictions", dietRestr));
            }
            else
                return await Task.FromResult(RedirectToAction("Index", "Home"));
        }

        public async Task<IActionResult> FoodPreferences()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetUserId(User);
                var userRestrictions = _restrictionContext.GetUserDislikedIngred(_restrictionContext.GetAll(), userId);
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

                var userSavedRecipes = _savedRecipeContext.GetAllRecipes(userId, _savedRecipeContext.GetAll());
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
                if (temp != null)
                {
                    _savedRecipeContext.RemoveSavedRecipe(temp);
                }
            }
            return await Task.FromResult(RedirectToAction("FavoriteRecipes"));
        }
    }
}