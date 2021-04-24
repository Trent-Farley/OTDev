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
            if (_db.GetAll().Count() < 1)
                await SeedDatabase();
            var randomRecipes = _db.GetRandomSix();
            return await Task.FromResult(View("Index", randomRecipes));
        }

        private async Task SeedDatabase()
        {
            var query = new Query
            {
                SearchType = "Random",
                Url = ApiConstants.RandomRecipesUrl,
                QueryName = "tags",
                QueryValue = "breakfast",
                Credentials = _config["SApiKey"]
            };
            await SearchApiAsync(query);
            query.QueryValue = "lunch";
            await SearchApiAsync(query);
            query.QueryValue = "dinner";
            await SearchApiAsync(query);
        }

        private async Task SearchApiAsync(Query query)
        {
            var possibleRecipes = _spnApi.SearchApi(query);
            if (possibleRecipes != null)
                foreach (var recipe in possibleRecipes)
                    if (!_db.GetAll().Any(t => t.Id == recipe.Id))
                        await _db.AddOrUpdateAsync(recipe);
        }

        [HttpPost]
        public async Task<IActionResult> RecipeDetails(Query query)
        {
            return await Task.FromResult(RedirectToAction("RecipeDetails", "Search", new { QueryValue = query.QueryValue }));
        }
    }
}