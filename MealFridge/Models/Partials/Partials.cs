using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Models
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
            Cuisine = r.Cuisine;
            GlutenFree = r.GlutenFree;
            DairyFree = r.DairyFree;
            VeryHealthy = r.VeryHealthy;
            Cheap = r.Cheap;
            Vegan = r.Vegan;
            Vegetarian = r.Vegetarian;
            LactoVeg = r.LactoVeg;
            OvoVeg = r.OvoVeg;
            Keto = r.Keto;
            Pescetarian = r.Pescetarian;
            Primal = r.Primal;
            Paleo = r.Paleo;
            Whole30 = r.Whole30;
        }
    }
}
