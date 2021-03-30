using MealFridge.Models;
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

        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _user;
        private readonly MealFridgeDbContext _db;
        // GET: SearchByName
        public SearchController(IConfiguration config, MealFridgeDbContext context, UserManager<IdentityUser> user)
        {
            _db = context;
            _configuration = config;
            _user = user;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user_id = _user.GetUserId(User);
                var temp = _db.Fridges.Where(f => f.AccountId == user_id);
                string ingredients = "";
                foreach (var f in temp)
                {
                    ingredients += _db.Ingredients.Where(a =>a.Id == f.IngredId).FirstOrDefault().Name + ", ";
                }
                if (ingredients.Length > 2) ingredients = ingredients.Substring(0, ingredients.Length - 2);
                Debug.WriteLine(ingredients);
                return View("Index", ingredients);
            }
            return View();
        }
    }
}
