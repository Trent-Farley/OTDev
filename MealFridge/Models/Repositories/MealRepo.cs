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

        public Meal GetMeal(string type, string userId, List<Ingredient> bans, List<Ingredient> dislikes)
        {
            //If they have meals to use, grab those
            var possibleMeals = _dbSet
                .Where(i => i.Type == type && i.AccountId == userId)
                .OrderBy(g => Guid.NewGuid())
                .ToList();
            //Else make a new meal from our db of recipes
            if (possibleMeals.Count < 5)
            {
                var recipes = _recipeSet.Where(r => r.Recipeingreds.Count > 0).Include(ri => ri.Recipeingreds).ToList();
                var recipe = new Recipe();
                recipes = GetRelevantRecipes(type, recipes);
                recipe = FindSafeRecipes(bans, dislikes, recipes)[0];
                var m = new Meal()
                {
                    Recipe = _recipeSet.FirstOrDefault(r => r.Id == recipe.Id),
                    RecipeId = recipe.Id,
                    AccountId = userId,
                    Type = type
                };

                _dbSet.Add(m);
                _context.SaveChanges();
                return m;
            }
            var collectedRecipes = possibleMeals.Select(r => r.Recipe).ToList();
            var mrecipe = FindSafeRecipes(bans, dislikes, collectedRecipes)[0];

            return _dbSet.FirstOrDefault(m => m.RecipeId == mrecipe.Id);
        }

        public Meal GetMeal(string type, string userId)
        {
            var possibleMeals = _dbSet
                .Where(i => i.Type == type && i.AccountId == userId)
                .OrderBy(g => Guid.NewGuid())
                .ToList();
            if (possibleMeals.Count < 1)
            {
                var recipes = _recipeSet.Where(r => r.Recipeingreds.Count > 0).Include(ri => ri.Recipeingreds).ToList();
                var recipe = new Recipe();
                recipes = GetRelevantRecipes(type, recipes);
                var m = new Meal()
                {
                    AccountId = userId,
                    Recipe = _recipeSet.FirstOrDefault(r => r.Id == recipes[0].Id),
                    RecipeId = recipes[0].Id,
                    Type = type
                };
                _dbSet.Add(m);
                _context.SaveChanges();
                return m;
            }
            return possibleMeals[0];
        }

        public List<Meal> GetMeals(string type, string userId, List<Ingredient> bans, List<Ingredient> dislikes, int days)
        {
            //Much like the GetMeal function, except this grabs the entire list.
            var possibleMeals = _dbSet
                .Where(i => i.Type == type && i.AccountId == userId)
                .OrderBy(g => Guid.NewGuid())
                .Include(r=> r.Recipe)
                .Include(ri => ri.Recipe.Recipeingreds)
                .ToList();
            if (possibleMeals.Count < days)
            {
                var recipes = _recipeSet.Where(r => r.Recipeingreds.Count > 0).Include(ri => ri.Recipeingreds).ToList();
                recipes = GetRelevantRecipes(type, recipes);
                recipes = FindSafeRecipes(bans, dislikes, recipes);
                foreach (var r in recipes)
                {
                    var temp = new Meal
                    {
                        AccountId = userId,
                        Type = type,
                        Recipe = _recipeSet.FirstOrDefault(l => l.Id == r.Id),
                        RecipeId = r.Id
                    };
                    if (!_dbSet.Any(ra => ra.AccountId == userId && r.Id == ra.RecipeId))
                    {
                        _dbSet.Add(temp);
                        _context.SaveChanges();
                    }
                    possibleMeals.Add(temp);
                }
            }
            return possibleMeals.AsQueryable().Include(r => r.Recipe).ToList();
        }

        public List<Meal> GetMeals(string type, string userId, int days)
        {
            var possibleMeals = _dbSet
                .Where(i => i.Type == type && i.AccountId == userId)
                .OrderBy(g => Guid.NewGuid())
                .ToList();
            if (possibleMeals.Count < days)
            {
                var recipes = _recipeSet.Where(r => r.Recipeingreds.Count > 0).Include(ri => ri.Recipeingreds).ToList();
                recipes = GetRelevantRecipes(type, recipes);
                foreach (var r in recipes)
                {
                    var temp = new Meal
                    {
                        AccountId = userId,
                        Type = type,
                        Recipe = _recipeSet.FirstOrDefault(l => l.Id == r.Id),
                        RecipeId = r.Id
                    };
                    if (!_dbSet.Any(ra => ra.AccountId == userId && r.Id == ra.RecipeId))
                    {
                        _dbSet.Add(temp);
                        _context.SaveChanges();
                    }
                    possibleMeals.Add(temp);
                }
            }
            return possibleMeals.Distinct().ToList();
        }

        private static List<Recipe> GetRelevantRecipes(string type, List<Recipe> recipes)
        {
            switch (type)
            {
                case "Breakfast":
                    recipes = recipes.Where(b => b.Breakfast == true).ToList();
                    break;

                case "Lunch":
                    recipes = recipes.Where(b => b.Lunch == true).ToList();
                    break;

                case "Dinner":
                    recipes = recipes.Where(b => b.Dinner == true).ToList();
                    break;
            }

            return recipes;
        }

        private static List<Recipe> FindSafeRecipes(List<Ingredient> bans, List<Ingredient> dislikes, List<Recipe> recipes)
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
            return found;
        }
    }
}