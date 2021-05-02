using MealFridge.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using MealFridge.Models.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MealFridge.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ISpnApiService _spnApi;

        //private readonly MealFridgeDbContext _db;
        private readonly IRecipeRepo _db;

        public HomeController(IConfiguration config, IRecipeRepo context, ISpnApiService service)
        {
            _config = config;
            _db = context;
            _spnApi = service;
        }

        public async Task<IActionResult> Index()
        {
            if (_db.GetAll().Count() <= 0)
                await SeedDatabase();
            var randomRecipes = _db.GetRandomSix();
            return await Task.FromResult(View("Index", randomRecipes));
        }

        private async Task SeedDatabase()
        {
            var query = new Query
            {
                SearchType = "Random",
                Url = ApiConstants.SearchByNameEndpoint,
                QueryValue = "breakfast",
                Credentials = _config["SApiKey"]
            };
            var possibleRecipes = _spnApi.SearchApi(query);
            query.QueryValue = "lunch";
            _spnApi.SearchApi(query).ForEach(i => possibleRecipes.Add(i));
            query.QueryValue = "dinner";
            _spnApi.SearchApi(query).ForEach(i => possibleRecipes.Add(i));
            if (possibleRecipes.Count > 0)
                await _db.SaveListOfRecipes(possibleRecipes.Distinct().ToList());
        }

        [HttpPost]
        public async Task<IActionResult> RecipeDetails(Query query)
        {
            return await Task.FromResult(RedirectToAction("RecipeDetails", "Search", new { query.QueryValue }));
        }
    }
}