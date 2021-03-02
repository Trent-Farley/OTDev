using HimalayanExpeditions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Pagination;
using cloudscribe.Web;
using cloudscribe.Pagination.Models;

namespace HimalayanExpeditions.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HimalayanExpeditionDbContext _context;

        public HomeController(ILogger<HomeController> logger, HimalayanExpeditionDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var articles = CreateArticle(_context.Expeditions.Include(p => p.Peak).ToList());
            return View("Index", articles);
        }
        public IActionResult AddExpeditionSubmit(ExpeditionAdder data)
        {
            if(ModelState.IsValid)
            {
                data.Expedition.OxygenUsed = data.OxyCheck;
                data.Expedition.TrekkingAgency = _context.TrekkingAgencies.Where(a => a.Id == data.Expedition.TrekkingAgencyId).FirstOrDefault();
                data.Expedition.Peak = _context.Peaks.Where(p => p.Id == data.Expedition.PeakId).FirstOrDefault();
                _context.Expeditions.Add(data.Expedition);
                _context.SaveChanges();
                return RedirectToAction("AddExpedition");
            }
            return View("AddExpedition");
        }
        public IActionResult AddExpedition()
        {
            ExpeditionAdder data = new ExpeditionAdder();
            data.Peaks = _context.Peaks.OrderBy(a => a.Name);
            data.TrekkingAgencies = _context.TrekkingAgencies.OrderBy(a => a.Name);
            return View(data);
        }

        private IEnumerable<string> CreateArticle(List<Expedition> expeditions)
        {
            var articles = new List<string>();
            expeditions.Reverse();
            foreach (var exp in expeditions)
            {
                var oxygen = (bool)exp.OxygenUsed ? "used oxygen" : "did not use oxygen";
                var succes = exp.TerminationReason.Contains("Success") ? "The expedition was a success!" : "The expedition failed because of " + exp.TerminationReason.ToLower();
                var article = $"In the {exp.Season} of {exp.StartDate.Value.Year}, a team " + $"of expeditioners embarked on their journey to summit {exp.Peak.Name}."
                  + $"During this adventure, the expeditioners {oxygen}. {succes}";
                articles.Add(article);
            }
            return articles;
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> FindPeaks(Search search, int pageNumber = 1)
        {
            if (ModelState.IsValid)
            {
                int pageSize = 20;

                var offset = (pageSize * pageNumber) - pageSize;
                IQueryable<Expedition> expeditionList = _context.Expeditions.Where(s => s.TerminationReason != "Success (main peak)").Include(p => p.Peak).OrderBy(p => p.Peak.Name)
                    .Skip(offset)
                    .Take(pageSize);
                var count = await _context.Expeditions.Where(s => s.TerminationReason != "Success (main peak)")
                    .Include(p => p.Peak)
                    .CountAsync();
                if (search.Season != "Any")
                {
                    expeditionList = expeditionList.Where(s => s.Season == search.Season).Include(p => p.Peak).OrderBy(p => p.Peak.Name)
                        .Skip(offset)
                        .Take(pageSize);
                    count = await _context.Expeditions.Where(s => s.TerminationReason != "Success (main peak)")
                    .Include(p => p.Peak)
                    .CountAsync();
                }
                var result = new PagedResult<Expedition>
                {
                    Data = await expeditionList.ToListAsync(),
                    TotalItems = count,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                search.ExpeditionList = result;

                search.Count = count;
                return View("FindPeaks", search);
            }
            else return View("FindPeaks", null);
        }
        public IActionResult Find()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Find(Search search, int? pageNumber)
        {

            int pageSize = 20;
            var currentPageNum = pageNumber.HasValue ? pageNumber.Value : 1;
            var offset = (pageSize * currentPageNum) - pageSize;
            var expeditionList = _context.Expeditions
                .Include(p => p.Peak);
            var count = expeditionList.Count();
            if (search.Year != null)
            {
                expeditionList = expeditionList.Where(c => c.Year == search.Year)
                    .Include(p => p.Peak);
                count = expeditionList.Count();

            }
            if (search.Peak != null)
            {
                expeditionList = expeditionList.Where(p => p.Peak.Name.Contains(search.Peak)).Include(p => p.Peak);
                count = expeditionList.Count();

            }
            if (search.Season != "Any")
            {
                expeditionList = expeditionList.Where(s => s.Season == search.Season)
                    .Include(p => p.Peak);
                count = expeditionList.Count();

            }
            if (search.TerminationReason != "Any")
            {
                expeditionList = expeditionList.Where(s => s.TerminationReason == search.TerminationReason)
                    .Include(p => p.Peak);
                count = expeditionList.Count();

            }

            var data = await expeditionList
                .Select(p => p)
                .Skip(offset)
                .Take(pageSize)
                .OrderBy(p => p.Peak.Name)
                .ToListAsync();

            var result = new PagedResult<Expedition>
            {
                Data = data,
                TotalItems = (count + pageSize - 1) / pageSize,
                PageNumber = currentPageNum,
                PageSize = pageSize
            };
            search.Count = count;
            search.ExpeditionList = result;
            return View("Find", search);
        }

        [HttpPost]
        public IActionResult Climber(Climber climber)
        {
            if (ModelState.IsValid)
            {
                _context.Climbers.Add(climber);
                _context.SaveChanges();
                return RedirectToAction("Climber");
            }
            return View("Index");
        }
        [HttpGet]
        public IActionResult Climber()
        {
            var temp = new SearchClimbers
            {
                ClimberList = _context.Climbers.ToList()
            };
            return View("Climber", temp);
        }

        [HttpGet]
        public IActionResult SearchClimber(SearchClimbers search)
        {
            if (ModelState.IsValid)
            {
                var temp = search.Name;
                var climberList = _context.Climbers.Where(c => c.Name.Contains(temp)).ToList();
                search.ClimberList = climberList;
                return View("Climber", search);
            }
            else
            {
                return View("Climber", null);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
