using MealFridge.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Repositories
{
    public class MealRepo : Repository<Meal>, IMealRepo
    {
        private DbSet<Recipe> _recipeSet;

        public MealRepo(MealFridgeDbContext ctx) : base(ctx)
        {
            _recipeSet = _context.Set<Recipe>();
        }

        public Meal GetMeal(DateTime mealTime, string userId, List<Ingredient> bans, List<Ingredient> dislikes)
        {
            var currentMeals = FindCurrentMeals(mealTime, userId);
            if (currentMeals.Count > 1)
                _dbSet.Remove(_dbSet.First(m => m.AccountId == userId && m.Day == mealTime));
            var type = GetType(mealTime);
            var recipes = _recipeSet.Where(r => r.Recipeingreds.Count > 0 && r.GetRecipeType() == type).Include(ri => ri.Recipeingreds).OrderBy(g => Guid.NewGuid()).ToList();
            var meals = FindRelevantMeals(recipes, userId, mealTime);
            meals = FindSafeMeals(bans, dislikes, meals);
            foreach (var meal in meals)
                if (!currentMeals.Contains(meal))
                {
                    var m = new Meal()
                    {
                        AccountId = userId,
                        Recipe = _recipeSet.FirstOrDefault(r => r.Id == meal.Recipe.Id),
                        RecipeId = meal.RecipeId,
                        MealType = type,
                        Day = mealTime
                    };
                    if (!_dbSet.Any(m => m.AccountId == userId && m.Day == mealTime))
                    {
                        _dbSet.Add(m);
                        _context.SaveChanges();
                        return m;
                    }
                }
            return null;
        }

        public Meal GetMeal(DateTime mealTime, string userId)
        {
            var currentMeals = FindCurrentMeals(mealTime, userId);
            if (currentMeals.Count > 1)
            {
                _dbSet.Remove(_dbSet.First(m => m.AccountId == userId && m.Day == mealTime));
            }

            var type = GetType(mealTime);
            var recipes = _recipeSet.Where(r => r.Recipeingreds.Count > 0).Include(ri => ri.Recipeingreds).OrderBy(g => Guid.NewGuid()).ToList();
            recipes = recipes.Where(r => r.GetRecipeType() == type).ToList();
            var meals = FindRelevantMeals(recipes, userId, mealTime);
            foreach (var meal in meals)
                if (!currentMeals.Contains(meal))
                {
                    var m = new Meal()
                    {
                        AccountId = userId,
                        Recipe = _recipeSet.FirstOrDefault(r => r.Id == meal.Recipe.Id),
                        RecipeId = meal.RecipeId,
                        MealType = type,
                        Day = mealTime
                    };
                    if (!_dbSet.Any(m => m.AccountId == userId && m.Day == mealTime))
                    {
                        _dbSet.Add(m);
                        _context.SaveChanges();
                    }
                    return m;
                }
            return null;
        }

        public List<Meal> GetMeals(DateTime mealTime, string userId, List<Ingredient> bans, List<Ingredient> dislikes, int days = 0, bool forceRefresh = false)
        {
            var currentMeals = FindCurrentMeals(mealTime, userId);
            if (!forceRefresh && currentMeals.Count > 0)
                return currentMeals.OrderBy(m => m.Recipe.Id).ToList();

            RemoveOldMeals(currentMeals);
            var type = GetType(mealTime);

            var recipes = _recipeSet.Where(r => r.Recipeingreds.Count > 0).Include(ri => ri.Recipeingreds).OrderBy(g => Guid.NewGuid()).ToList();
            var meals = FindRelevantMeals(recipes, userId, mealTime);
            meals = FindSafeMeals(bans, dislikes, meals);
            var newMeals = new List<Meal>();
            for (var i = 0; i < days; ++i)
            {
                var temp = new Meal
                {
                    AccountId = userId,
                    MealType = type,
                    Recipe = _recipeSet.First(r => r.Id == meals[i].Recipe.Id),
                    RecipeId = _recipeSet.First(r => r.Id == meals[i].Recipe.Id).Id,
                    Day = DateTime.Today + TimeSpan.FromDays(i) + mealTime.TimeOfDay,
                };
                if (!_dbSet.Any(m => m.AccountId == userId && m.Day == temp.Day))
                {
                    //_dbSet.Add(temp);
                    //_context.SaveChanges();
                }
                newMeals.Add(temp);
            }
            return newMeals.OrderBy(m => m.Recipe.Id).ToList();
        }

        public List<Meal> GetMeals(DateTime mealTime, string userId, int days = 0, bool forceRefresh = false)
        {
            var currentMeals = FindCurrentMeals(mealTime, userId);
            if (!forceRefresh && currentMeals.Count > 0)
                return currentMeals;

            RemoveOldMeals(currentMeals);
            var type = GetType(mealTime);

            var recipes = _recipeSet.Where(r => r.Recipeingreds.Count > 0).Include(ri => ri.Recipeingreds).OrderBy(g => Guid.NewGuid()).ToList();
            var meals = FindRelevantMeals(recipes, userId, mealTime);
            var newMeals = new List<Meal>();
            for (var i = 0; i < days; ++i)
            {
                var temp = new Meal
                {
                    AccountId = userId,
                    MealType = type,
                    Recipe = _recipeSet.First(r => r.Id == meals[i].Recipe.Id),
                    RecipeId = _recipeSet.First(r => r.Id == meals[i].Recipe.Id).Id,
                    Day = DateTime.Today + TimeSpan.FromDays(i) + mealTime.TimeOfDay,
                };
                if (!_dbSet.Any(m => m.AccountId == userId && m.Day == temp.Day))
                {
                    _dbSet.Add(temp);
                    _context.SaveChanges();
                }
                newMeals.Add(temp);
            }
            return newMeals;
        }

        private void RemoveOldMeals(List<Meal> meals)
        {
            foreach (var m in meals)
            {
                _dbSet.Remove(_dbSet.First(om => om.Day == m.Day && om.AccountId == m.AccountId));
            }
            _context.SaveChanges();
        }

        private List<Meal> FindRelevantMeals(List<Recipe> recipes, string userId, DateTime mealTime)
        {
            var type = GetType(mealTime);
            switch (type)
            {
                case "Breakfast":
                    recipes = recipes.Where(b => b.Breakfast == true).OrderBy(g => Guid.NewGuid()).ToList();
                    break;

                case "Lunch":
                    recipes = recipes.Where(b => b.Lunch == true).OrderBy(g => Guid.NewGuid()).ToList();
                    break;

                case "Dinner":
                    recipes = recipes.Where(b => b.Dinner == true).OrderBy(g => Guid.NewGuid()).ToList();
                    break;
            }
            var meals = new List<Meal>();
            recipes.ForEach(r => meals.Add(new Meal { Recipe = r, AccountId = userId, Day = mealTime, MealType = type, RecipeId = r.Id }));
            return meals;
        }

        /// <summary>
        /// Get which meal this is for, Breakfast, Lunch or Dinner
        /// </summary>
        /// <param name="mealTime">MealTime needs to have 3 specific times: 08 AM for Breakfast, 12 for lunch and 5 for dinner. Everything
        /// else will be deemed as a snack</param>
        /// <returns>A string with the current meal</returns>
        private string GetType(DateTime mealTime)   // Super fragile
        {
            return mealTime.ToString("hh") switch
            {
                "08" => "Breakfast",
                "12" => "Lunch",
                "05" => "Dinner",
                _ => "Snack",
            };
        }

        /// <summary>
        /// Gets all the meals that a user has. Also releases any meals that
        /// are no longer being used.
        /// </summary>
        /// <param name="currentDay">The current date to get the meal</param>
        /// <param name="userId">the user's id</param>
        /// <returns>A list of current meals</returns>
        private List<Meal> FindCurrentMeals(DateTime currentDay, string userId)
        {
            var allUserMeals = _dbSet
              .Where(i => i.AccountId == userId && i.Day > currentDay && i.Day.Hour == currentDay.Hour)
              .Include(r => r.Recipe)
              .ToList();
            if (allUserMeals.Count < 1)
                return allUserMeals;

            foreach (var meal in allUserMeals)
            {
                if (meal.Day.Day < currentDay.Day)  // Removes Meal if it was yesterday
                {
                    _dbSet.Remove(_dbSet.First(m => m.Day == meal.Day && meal.AccountId == userId));
                    _context.SaveChanges();
                    allUserMeals.Remove(meal);
                }
            }
            return allUserMeals;
        }

        private static List<Meal> FindSafeMeals(List<Ingredient> bans, List<Ingredient> dislikes, List<Meal> meals)
        {
            var found = new List<Meal>();
            foreach (var r in meals)
            {
                foreach (var i in r.Recipe.Recipeingreds)
                    if (bans.Contains(i.Ingred))
                        continue;
                    else if (dislikes.Contains(i.Ingred))
                        r.Recipe.Dislike = true;
                found.Add(r);
            }
            return found;
        }
    }
}