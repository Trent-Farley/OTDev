using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface IMealRepo : IRepository<Meal>
    {
        public List<Meal> GetMeals(DateTime mealTime, string userId, List<Ingredient> bans, List<Ingredient> dislikes, int days = 0, bool forceRefresh = false);

        public List<Meal> GetMeals(DateTime mealTime, string userId, int days = 0, bool forceRefresh = false);

        public Meal GetMeal(DateTime mealTime, string userId, List<Ingredient> bans, List<Ingredient> dislikes);

        public Meal GetMeal(DateTime mealTime, string userId);

        public List<Meal> GetAllMealsWithRecipes();
    }
}