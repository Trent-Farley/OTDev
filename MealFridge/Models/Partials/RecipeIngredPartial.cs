using MealFridge.Models.Interfaces;
using MealFridge.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models
{
    public partial class Recipeingred : IFoodItem
    {
        public static List<Recipeingred> CreateRecipeIngred(JArray ingredients, int recipeId, List<Ingredient> parsedIngreds)
        {
            var retingredients = new List<Recipeingred>();
            foreach (var ing in ingredients)
            {
                if (!int.TryParse(ing["id"].ToString(), out int ingId))
                    continue;
                if (retingredients.Any(i => i.IngredId == ingId))
                {
                    retingredients.First(i => i.IngredId == ingId).Amount += ing["amount"]?.Value<double>();
                    continue;
                }
                var newRI = new Recipeingred
                {
                    RecipeId = recipeId,
                    IngredId = ingId,
                    Amount = ing["amount"]?.Value<double>(),
                    ServingUnit = ing["unit"]?.Value<string>(),
                    Ingred = parsedIngreds.FirstOrDefault(i => i.Id == ingId)
                };
                var nutrients = ing["nutrients"].ToList();
                JsonParser.GetNutrition(newRI, nutrients);
                retingredients.Add(newRI);
            }
            return retingredients;
        }
    }
}