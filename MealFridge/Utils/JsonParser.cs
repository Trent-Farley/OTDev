using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MealFridge.Models;
using MealFridge.Models.Interfaces;
using Newtonsoft.Json.Linq;

namespace MealFridge.Utils
{
    public static class JsonParser
    {
        public static IFoodItem GetNutrition(IFoodItem item, List<JToken> nutrition)
        {
            foreach (var n in nutrition)
            {
                switch (n["name"].ToString())
                {
                    case "Calories":
                        item.Calories = (float)n["amount"];
                        break;

                    case "Saturated Fat":
                        item.SatFat = (float)n["amount"];
                        break;

                    case "Fat":
                        item.TotalFat = (float)n["amount"];
                        break;

                    case "Net Carbohydrates":
                        item.NetCarbs = (float)n["amount"];
                        break;

                    case "Carbohydrates":
                        item.Carbs = (float)n["amount"];
                        break;

                    case "Cholesterol":
                        item.Cholesterol = (float)n["amount"];
                        break;

                    case "Sodium":
                        item.Sodium = (float)n["amount"];
                        break;

                    case "Protein":
                        item.Protein = (float)n["amount"];
                        break;

                    case "Sugar":
                        item.Sugar = (float)n["amount"];
                        break;

                    default:
                        break;
                }
            }
            return item;
        }

        public static List<Ingredient> IngredientList(JArray ingredients)
        {
            var result = new List<Ingredient>();
            foreach (var ing in ingredients)
            {
                if (ing["id"] == null || ing["id"].Type == JTokenType.Null)
                    continue;
                else
                {
                    Ingredient Ingred = Ingredient.CreateIngredient(ing);
                    if (Ingred != null)
                        result.Add(Ingred);
                }
            }
            return result;
        }
    }
}