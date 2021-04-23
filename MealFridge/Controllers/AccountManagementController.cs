using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MealFridge.Models;
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
        private readonly MealFridgeDbContext _context;
        private readonly UserManager<IdentityUser> _user;

        public AccountManagementController(IConfiguration config, MealFridgeDbContext context, UserManager<IdentityUser> user)
        {
            _configuration = config;
            _context = context;
            _user = user;
        }

        public async Task<ActionResult> Index()
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
                var userSavedRecipes = _context.Savedrecipes.Where(f => f.AccountId == userId).Include(r => r.Recipe).ToList();
                return await Task.FromResult(View("FavoriteRecipes", userSavedRecipes.ToList()));
            }
            return await Task.FromResult(RedirectToAction("Index", "Home"));
        }

        public async Task<ActionResult> Delete(string account_id, int recipe_id)
        {
            var temp = _context.Savedrecipes.Where(a => a.AccountId == account_id).ToList();
            foreach(var i in temp)
            {
                if (i.RecipeId == recipe_id)
                {
                    _context.Savedrecipes.Remove(i);
                }
            }
            _context.SaveChanges();
            return await Task.FromResult(RedirectToAction("FavoriteRecipes"));
        }
    }
}