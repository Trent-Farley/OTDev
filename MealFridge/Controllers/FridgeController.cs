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
                return View("Index", temp.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        


    }
}
