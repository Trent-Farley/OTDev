using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TastyMeals.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult GetStarted()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
    }
}