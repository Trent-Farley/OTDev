using HimalayanExpeditions.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HimalayanExpeditions.Controllers
{
    [Route("api/{action}")]
    [ApiController]
    public class ApiController : Controller
    {
        private readonly HimalayanExpeditionDbContext db;
        public ApiController(HimalayanExpeditionDbContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult count()
        {
            var unclimbed = db.Peaks.Where(e => e.Expeditions.ToList().Count() == 0).ToList().Count();
            var last = db.Expeditions.OrderByDescending(e => e.StartDate).First();
            last.TrekkingAgency = db.TrekkingAgencies.Where(p => p.Id == last.TrekkingAgencyId).FirstOrDefault();
            last.Peak = db.Peaks.Where(p => p.Id == last.PeakId).FirstOrDefault();
            return Json(new { val = db.Expeditions.ToList().Count().ToString() + " Expeditions to Date, with " + unclimbed + " yet to be climbed!\n" +
                "The newest Expedition was " + last.TrekkingAgency.Name + "'s trip to " + last.Peak.Name + " on " + last.StartDate.ToString() });
            }
    }
}
