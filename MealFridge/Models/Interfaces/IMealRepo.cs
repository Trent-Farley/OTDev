using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface IMealRepo : IRepository<Meal>
    {
        public List<Meal> GetMeals(string type, string userId, List<Ingredient> bans, List<Ingredient> dislikes, int days);

        public List<Meal> GetMeals(string type, string userId, int days);

        public Meal GetMeal(string type, string userId, List<Ingredient> bans, List<Ingredient> dislikes);

        public Meal GetMeal(string type, string userId);
    }
}