using System;
using System.Collections.Generic;

namespace MealFridge.Models
{
    public partial class Meal
    {
        public static List<Meal> CreateMealsFromRecipes(List<Recipe> recipes)
        {
            var meals = new List<Meal>();
            foreach (var recipe in recipes)
            {
                var type = recipe.Breakfast.Value ? "Breakfast"
                    : recipe.Lunch.Value ? "Lunch"
                    : recipe.Dinner.Value ? "Dinner"
                    : recipe.Snack.Value ? "Snack" : "Dessert";
                meals.Add(new Meal
                {
                    Recipe = recipe,
                    MealString = type
                });
            }
            return meals;
        }
    }
}