using MealFridge.Models.Interfaces;
using MealFridge.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models
{
    public partial class Recipe : IFoodItem
    {
        public static Recipe CreateRecipe(JToken recipe)
        {
            var createdRecipe = new Recipe();
            try
            {
                createdRecipe.Id = recipe["id"].Value<int>();
                createdRecipe.Title = recipe["title"].Value<string>();
                createdRecipe.Image = "https://spoonacular.com/recipeImages/" + recipe["id"].Value<int>() + "-556x370." + recipe["imageType"].Value<string>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n While parsing required Recipe properties, the following error occurred for {createdRecipe.Title}");
                Console.WriteLine($"\n {e.ToString().Substring(0, 100)}");
                Console.WriteLine(recipe.ToString());
                return null;
            }
            try
            {
                var ingredients = new List<Ingredient>();
                try
                {
                    ingredients = JsonParser.IngredientList(recipe["extendedIngredients"].Value<JArray>());
                }
                catch { }
                createdRecipe.Recipeingreds = Recipeingred.CreateRecipeIngred(recipe["nutrition"]["ingredients"].Value<JArray>(), recipe["id"].Value<int>(), ingredients);
                createdRecipe.Summery = recipe["sourceUrl"].Value<string>();

                var nutrients = recipe["nutrition"]["nutrients"].ToList();
                JsonParser.GetNutrition(createdRecipe, nutrients);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Recipe {createdRecipe.Title} had no nutrition or ingredients");
                Console.WriteLine("````````````\n" + recipe.ToString() + "\n````````````````\n ");
            }
            return createdRecipe;
        }
    }
}