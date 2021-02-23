using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MealFridge.Controllers
{
    public class AccountManagementController : Controller
    {
        // GET: AccountManagement
        
        public ActionResult Index() 
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
        public ActionResult FavoriteRecipes ()
        {
            return View();
        }
    }
}