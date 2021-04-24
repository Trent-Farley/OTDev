using System.Collections.Generic;

namespace MealFridge.Models.ViewModels
{
    public class Meals
    {
        public List<Meal> Breakfast { get; set; }
        public List<Meal> Lunch { get; set; }
        public List<Meal> Dinner { get; set; }
        public List<string> Days { get; set; }
        public int DaysCount { get; set; }
    }
}