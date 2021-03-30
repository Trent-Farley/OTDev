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
        // GET: SearchByName
        public FridgeController(IConfiguration config, MealFridgeDbContext context, UserManager<IdentityUser> user)
        {
            _configuration = config;
            _context = context;
            _user = user;
        }
        
        
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user_id = _user.GetUserId(User);
                var temp = _context.Fridges.Where(f => f.AccountId == user_id);
                foreach (var f in temp)
                {
                    f.Ingred = _context.Ingredients.Find(f.IngredId);
                }
                return View("Index", temp.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult AddItem(int id, int amount)
        {
            var newItem = new Fridge();
            newItem.AccountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            newItem.IngredId = id;
            newItem.Quantity = amount;
            if (!_context.Fridges.Any(item => item == newItem))
            {
                _context.Add(newItem);
                _context.SaveChanges();
            }
            return Redirect("/");
        }
        public IActionResult RemoveItem(int id)
        {
            var newItem = new Fridge();
            newItem.AccountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            newItem.IngredId = id;
            if (_context.Fridges.Any(item => item == newItem))
            {
                _context.Remove(newItem);
                _context.SaveChanges();
            }
            return Redirect("/");
        }
        public IActionResult UpdateItem(int id, int amount)
        {
            var newItem = new Fridge();
            newItem.AccountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            newItem.IngredId = id;
            newItem.Quantity = amount;
            if (_context.Fridges.Any(item => item == newItem))
            {
                _context.Update(newItem);
                _context.SaveChanges();
            }
            return Redirect("/");
        }
    }
}
