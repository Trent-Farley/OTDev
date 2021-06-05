using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Utils
{
    public static class ApiConstants
    {
        public static string BaseUrl { get; } = "https://api.spoonacular.com/";
        public static string BaseRecipeURL { get; } = BaseUrl + "/recipes";
        public static string BaseIngredientsUrl { get; } = BaseUrl + "/ingredients";
        public static string IngredientImageUrl { get; } = "https://spoonacular.com/cdn/ingredients_500x500/";

        public static string GenerateMealPlanURL { get; } = BaseUrl + "/mealplanner/generate";

        public static string BuildRecipeImageString(string id, string imageType)
        {
            return "https://spoonacular.com/recipeImages/" + id + "-556x370." + imageType;
        }

        public static string SearchByNameEndpoint { get; } = "https://api.spoonacular.com/recipes/complexSearch";
        public static string SearchByIngredientEndpoint { get; } = "https://api.spoonacular.com/recipes/findByIngredients";
        public static string SearchByRecipeEndpoint { get; } = "https://api.spoonacular.com/recipes/{id}/information";
        public static string RandomRecipesUrl { get; } = BaseUrl + "recipes/random";
        public static string RandomRecipeAmount { get; } = "&number=100";
    }
}