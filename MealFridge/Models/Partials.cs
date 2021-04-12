using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models
{
    public partial class Recipe
    {
        public void UpdateRecipe(Recipe r)
        {
            Servings = r.Servings;
            Minutes = r.Minutes;
            Summery = r.Summery;
            Instructions = r.Instructions;
            Cost = r.Cost;
            ServingSize = r.ServingSize;
            ServingUnit = r.ServingUnit;
            Calories = r.Calories;
            TotalFat = r.TotalFat;
            SatFat = r.SatFat;
            Carbs = r.Carbs;
            NetCarbs = r.NetCarbs;
            Sugar = r.Sugar;
            Cholesterol = r.Cholesterol;
            Sodium = r.Sodium;
            Protein = r.Protein;
            Breakfast = r.Breakfast;
            Lunch = r.Lunch;
            Dinner = r.Dinner;
            Dessert = r.Dessert;
            Snack = r.Snack;
        }
    }
}
