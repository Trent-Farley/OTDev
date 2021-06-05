using TastyMeals.Models.Interfaces;
using TastyMeals.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Models.Repositories
{
    public class MealRepo : Repository<Meal>, IMealRepo
    {
        private DbSet<Recipe> _recipeSet;

        public MealRepo(TastyMealsDbContext ctx) : base(ctx)
        {
            _recipeSet = _context.Set<Recipe>();
        }

        public Meal GetMeal(DateTime mealTime, string userId, List<Ingredient> bans, List<Ingredient> dislikes)
        {
            var currentMeals = new List<Meal>();
            currentMeals = FindCurrentMeals(mealTime, userId);
            var oldMeal = _dbSet.FirstOrDefault(m => m.AccountId == userId && m.Day == mealTime);
            var type = GetMealType(mealTime);

            if (oldMeal == null)
            {
                oldMeal = new Meal
                {
                    AccountId = userId,
                    Day = mealTime,
                    MealType = type,
                };
            }
            var recipes = _recipeSet
                .Where(r => r.Recipeingreds.Count > 0)
                .Include(ri => ri.Recipeingreds)
                .ThenInclude(i => i.Ingred)
                .OrderBy(g => Guid.NewGuid())
                .ToList();
            var meals = FindRelevantMeals(recipes, userId, mealTime);
            meals = FindSafeMeals(bans, dislikes, meals);
            foreach (var meal in meals.AsEnumerable().OrderBy(g => Guid.NewGuid()).Reverse().ToList())
                if (currentMeals.Count() != 0)
                {
                    if (!currentMeals.Select(m => m.RecipeId).ToList().Contains(meal.Id) && meal.Id != oldMeal.RecipeId)
                    {
                        oldMeal.Recipe = _recipeSet.FirstOrDefault(r => r.Id == meal.Id);
                        oldMeal.RecipeId = meal.Id;
                        _dbSet.Update(oldMeal);
                        _context.SaveChanges();
                        return oldMeal;
                    }
                }
                else
                {
                    oldMeal.Recipe = _recipeSet.FirstOrDefault(r => r.Id == meal.Id);
                    oldMeal.RecipeId = meal.Id;
                    _dbSet.Update(oldMeal);
                    _context.SaveChanges();
                    return oldMeal;
                }

            return null;
        }

        public Meal GetMeal(DateTime mealTime, string userId)
        {
            var currentMeals = FindCurrentMeals(mealTime, userId);
            var oldMeal = _dbSet.FirstOrDefault(m => m.AccountId == userId && m.Day == mealTime);

            if (currentMeals.Count > 1)
            {
                _dbSet.Remove(oldMeal);
                _context.SaveChanges();
            }

            var type = GetMealType(mealTime);
            var recipes = _recipeSet.Where(r => r.Recipeingreds.Count > 0)
             .Include(ri => ri.Recipeingreds)
             .ThenInclude(i => i.Ingred)
             .OrderBy(g => Guid.NewGuid())
             .ToList();
            var newMeals = new List<Meal>();
            var relevantRecipes = FindRelevantMeals(recipes, userId, mealTime);
            foreach (var meal in relevantRecipes.AsEnumerable().OrderBy(g => Guid.NewGuid()).Reverse().ToList())
            {
                if (!newMeals.Select(m => m.RecipeId).ToList().Contains(meal.Id))
                {
                    var m = new Meal()
                    {
                        AccountId = userId,
                        Recipe = _recipeSet.FirstOrDefault(r => r.Id == meal.Id),
                        RecipeId = meal.Id,
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
            }
            return null;
        }

        public List<Meal> GetMeals(DateTime mealTime, string userId, List<Ingredient> bans, List<Ingredient> dislikes, int days = 0, bool forceRefresh = false, MealFilter filter = null)
        {
            var currentMeals = FindCurrentMeals(mealTime, userId);
            if (!forceRefresh)
                return currentMeals.OrderBy(m => m.Recipe.Id).ToList();

            var type = GetMealType(mealTime);

            var recipes = _recipeSet
                .Where(r => r.Recipeingreds.Count > 0)
             .Include(ri => ri.Recipeingreds)
             .ThenInclude(i => i.Ingred)
             .OrderBy(g => Guid.NewGuid())
             .Distinct()
             .ToList();
            var relevantRecipes = FindRelevantMeals(recipes, userId, mealTime, filter);

            var safeRecipes = FindSafeMeals(bans, dislikes, relevantRecipes);

            var newMeals = new List<Meal>();
            if (safeRecipes.Count() == 0)
                return newMeals;
            var total = 0;
            var randomized = safeRecipes.AsEnumerable().OrderBy(g => Guid.NewGuid()).Reverse().ToList();
            for (var i = 0; i < days; ++i)
            {
                randomized = safeRecipes.AsEnumerable().OrderBy(g => Guid.NewGuid()).Reverse().ToList();
                var meal = new Recipe();
                ++total;

                if (total <= randomized.Count() - 1)
                {
                    meal = randomized[total];
                }
                else
                {
                    meal = randomized[0];
                    total = 0;
                }

                var temp = new Meal
                {
                    AccountId = userId,
                    MealType = type,
                    Recipe = _recipeSet.First(r => r.Id == meal.Id),
                    RecipeId = _recipeSet.First(r => r.Id == meal.Id).Id,
                    Day = DateTime.Today + TimeSpan.FromDays(i) + mealTime.TimeOfDay,
                };
                if (!_dbSet.Any(m => m.Day == temp.Day))
                {
                    _dbSet.Add(temp);
                    _context.SaveChanges();
                    newMeals.Add(temp);
                }
            }
            return newMeals;
        }

        public List<Meal> GetMeals(DateTime mealTime, string userId, int days = 0, bool forceRefresh = false)
        {
            var currentMeals = FindCurrentMeals(mealTime, userId);
            if (!forceRefresh)
                return currentMeals;

            var type = GetMealType(mealTime);

            var recipes = _recipeSet
                .Where(r => r.Recipeingreds.Count > 0)
                .Include(ri => ri.Recipeingreds)
                .ThenInclude(i => i.Ingred)
                .OrderBy(g => Guid.NewGuid())
                .Distinct()
                .ToList();
            var meals = FindRelevantMeals(recipes, userId, mealTime);

            var newMeals = new List<Meal>();
            var i = 0;
            foreach (var meal in meals.AsEnumerable().OrderBy(g => Guid.NewGuid()).Reverse().ToList())
            {
                if (!newMeals.Select(m => m.RecipeId).ToList().Contains(meal.Id))
                {
                    var temp = new Meal
                    {
                        AccountId = userId,
                        MealType = type,
                        Recipe = _recipeSet.First(r => r.Id == meal.Id),
                        RecipeId = _recipeSet.First(r => r.Id == meal.Id).Id,
                        Day = DateTime.Today + TimeSpan.FromDays(i) + mealTime.TimeOfDay,
                    };
                    if (!_dbSet.Any(m => m.AccountId == userId && m.Day == temp.Day))
                    {
                        _dbSet.Add(temp);
                    }
                    newMeals.Add(temp);
                    ++i;
                    _context.SaveChanges();
                    if (i == days)
                        return newMeals;
                }
            }
            return newMeals;
        }

        public void RemoveOldMeals()
        {
            _context.RemoveRange(_dbSet);
            _context.SaveChanges();
        }

        private List<Recipe> FindRelevantMeals(List<Recipe> recipes, string userId, DateTime mealTime, MealFilter filter = null)
        {
            var type = GetMealType(mealTime);
            if (filter == null)
                recipes = FindRelevantMeals(recipes, type);
            else
                recipes = FindFilteredtMeals(recipes, type, filter);

            return recipes;
        }

        private static List<Recipe> FindRelevantMeals(List<Recipe> recipes, string type)
        {
            switch (type)
            {
                case "Breakfast":
                    recipes = recipes.Where(b => b.Breakfast == true)
                        .OrderBy(g => Guid.NewGuid())
                        .ToList();
                    break;

                case "Lunch":
                    recipes = recipes.Where(b => b.Lunch == true)
                        .OrderBy(g => Guid.NewGuid())
                        .ToList();
                    break;

                case "Dinner":
                    recipes = recipes.Where(b => b.Dinner == true)
                        .OrderBy(g => Guid.NewGuid())
                        .ToList();
                    break;
            }

            return recipes;
        }

        private static List<Recipe> Filter(List<Recipe> recipes, string type, MealFilter filter)
        {
            return recipes
                  .Where(s => (s.Servings ?? 0) <= (filter.Servings))
                  .Where(m => (m.Minutes ?? 0) <= (filter.Minutes))
                  .Where(c => (c.Calories ?? 0) <= (filter.Calories))
                  .Where(t => (t.TotalFat ?? 0) <= (filter.TotalFat))
                  .Where(s => (s.SatFat ?? 0) <= (filter.SatFat))
                  .Where(ca => (ca.Carbs ?? 0) <= (filter.Carbs))
                  .Where(n => (n.NetCarbs ?? 0) <= (filter.NetCarbs))
                  .Where(s => (s.Sugar ?? 0) <= (filter.Sugar))
                  .Where(ch => (ch.Cholesterol ?? 0) <= (filter.Cholesterol))
                  .Where(s => (s.Sodium ?? 0) <= (filter.Sodium))
                  .Where(p => (p.Protein ?? 0) <= (filter.Protein))
                  .Where(gl => filter.GlutenFree == false || gl.GlutenFree.GetValueOrDefault() == true)
                  .Where(df => filter.DairyFree == false || df.DairyFree.GetValueOrDefault() == true)
                  .Where(k => filter.Keto == false || k.Keto.GetValueOrDefault() == true)
                  .Where(lv => filter.LactoVeg == false || lv.LactoVeg.GetValueOrDefault() == true)
                  .Where(p => filter.Pescetarian == false || p.Pescetarian.GetValueOrDefault() == true)
                  .Where(p => filter.Paleo == false || p.Paleo.GetValueOrDefault() == true)
                  .Where(pr => filter.Primal == false || pr.Primal.GetValueOrDefault() == true)
                  .Where(wt => filter.Whole30 == false || wt.Whole30.GetValueOrDefault() == true)
                  .Where(vg => filter.Vegan == false || vg.Vegan.GetValueOrDefault() == true)
                  .Where(v => filter.VeryHealthy == false || v.VeryHealthy.GetValueOrDefault() == true)
                  .Where(c => filter.Cheap == false || c.Cheap.GetValueOrDefault() == true)
                  .Where(ve => filter.Vegetarian == false || ve.Vegetarian.GetValueOrDefault() == true)
                  .OrderBy(g => Guid.NewGuid())
                  .Distinct()
                  .ToList();
        }

        private static List<Recipe> FindFilteredtMeals(List<Recipe> recipes, string type, MealFilter filter)
        {
            switch (type)
            {
                case "Breakfast":
                    recipes = Filter(recipes, type, filter).Where(b => b.Breakfast == true).ToList();
                    break;

                case "Lunch":
                    recipes = Filter(recipes, type, filter).Where(l => l.Lunch == true).ToList();
                    break;

                case "Dinner":
                    recipes = Filter(recipes, type, filter).Where(b => b.Dinner == true).ToList();
                    break;
            }

            return recipes;
        }

        /// <summary>
        /// Get which meal this is for, Breakfast, Lunch or Dinner
        /// </summary>
        /// <param name="mealTime">MealTime needs to have 3 specific times: 08 AM for Breakfast, 12 for lunch and 5 for dinner. Everything
        /// else will be deemed as a snack</param>
        /// <returns>A string with the current meal</returns>
        private string GetMealType(DateTime mealTime)   // Super fragile
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
            var type = GetMealType(currentDay);
            var allUserMeals = _dbSet
              .Where(i => i.AccountId == userId && i.MealType == type)
              .Include(r => r.Recipe)
              .ToList();

            return allUserMeals;
        }

        private static List<Recipe> FindSafeMeals(List<Ingredient> bans, List<Ingredient> dislikes, List<Recipe> recipes)
        {
            var found = new List<Recipe>();
            foreach (var r in recipes)
            {
                foreach (var i in r.Recipeingreds)
                    if (bans.Contains(i.Ingred))
                        continue;
                    else if (dislikes.Contains(i.Ingred))
                        r.Dislike = true;

                found.Add(r);
            }
            return found.OrderBy(t => Guid.NewGuid()).ToList();
        }

        public List<Meal> GetAllMealsWithRecipes()
        {
            return _dbSet
                .Include(r => r.Recipe)
                .ThenInclude(r => r.Recipeingreds)
                .ThenInclude(i => i.Ingred)
                .OrderBy(t => t.Day)
                .ToList();
        }
    }
}