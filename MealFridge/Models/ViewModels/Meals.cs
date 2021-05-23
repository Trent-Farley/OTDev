using System.Collections.Generic;

namespace MealFridge.Models.ViewModels
{
#nullable enable

    public class Meals
    {
        public List<Meal> Breakfast { get; set; } = new List<Meal>();
        public List<Meal> Lunch { get; set; } = new List<Meal>();
        public List<Meal> Dinner { get; set; } = new List<Meal>();
        public int DaysCount { get; set; }
    }
}