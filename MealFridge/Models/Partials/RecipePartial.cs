using MealFridge.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models
{
    public partial class Recipe : IFoodItem
    {
        [NotMapped]
        public bool Dislike { get; set; }

        [NotMapped]
        public bool Banned { get; set; }

        public static Recipe GetTotalDay(Recipe breakfast, Recipe Lunch, Recipe dinner)
        {
            var totalRecipe = new Recipe
            {
                Calories = (double?)Math.Round((decimal)((breakfast.Calories ?? 0.0) + (Lunch.Calories ?? 0.0) + (dinner.Calories ?? 0.0))),
                TotalFat = (double?)Math.Round((decimal)((breakfast.TotalFat ?? 0) + (Lunch.TotalFat ?? 0) + (dinner.TotalFat ?? 0))),
                SatFat = (double?)Math.Round((decimal)((breakfast.SatFat ?? 0) + (Lunch.SatFat ?? 0) + (dinner.SatFat ?? 0))),
                Carbs = (double?)Math.Round((decimal)((breakfast.Carbs ?? 0) + (Lunch.Carbs ?? 0) + (dinner.Carbs ?? 0))),
                NetCarbs = (double?)Math.Round((decimal)((breakfast.NetCarbs ?? 0) + (Lunch.NetCarbs ?? 0) + (dinner.NetCarbs ?? 0))),
                Sugar = (double?)Math.Round((decimal)((breakfast.Sugar ?? 0) + (Lunch.Sugar ?? 0) + (dinner.Sugar ?? 0))),
                Cholesterol = (double?)Math.Round((decimal)((breakfast.Cholesterol ?? 0) + (Lunch.Cholesterol ?? 0) + (dinner.Cholesterol ?? 0))),
                Sodium = (double?)Math.Round((decimal)((breakfast.Sodium ?? 0) + (Lunch.Sodium ?? 0) + (dinner.Sodium ?? 0))),
                Protein = (double?)Math.Round((decimal)((breakfast.Protein ?? 0) + (Lunch.Protein ?? 0) + (dinner.Protein ?? 0)))
            };
            return totalRecipe;
        }
    }
}