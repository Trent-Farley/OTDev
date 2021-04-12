using MealFridge.Models.Interfaces;
using MealFridge.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models
{
    public partial class Ingredient : IFoodItem
    {
        public static Ingredient CreateIngredient(JToken ingredient)
        {
            Console.WriteLine(ingredient.ToString());

            var createdIng = new Ingredient();
            try
            {
                createdIng.Id = ingredient["id"].Value<int>();
                createdIng.Image = "https://spoonacular.com/cdn/ingredients_500x500/" + ingredient["image"];
                createdIng.Name = ingredient["name"].Value<string>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n While parsing required ingredient properties, the following error occurred for {createdIng.Name}");
                Console.WriteLine($"\n {e.ToString().Substring(0, 100)}");
                return null;
            }
            try
            {
                var nutrients = ingredient["nutrition"]["nutrients"].ToList();
                JsonParser.GetNutrition(createdIng, nutrients);
                createdIng.Aisle = ingredient["aisle"].Value<string>();
            }
            catch
            {
                Console.WriteLine("Ingredient had no nutrients or aisle");
            }
            return createdIng;
        }
    }
}