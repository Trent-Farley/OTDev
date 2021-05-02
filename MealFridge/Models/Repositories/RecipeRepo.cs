using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealFridge.Models;
using MealFridge.Models.Interfaces;
using MealFridge.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MealFridge.Models.Repositories
{
    public class RecipeRepo : Repository<Recipe>, IRecipeRepo
    {
        public RecipeRepo(MealFridgeDbContext ctx) : base(ctx)
        {
            _dbSet.Include(ri => ri.Recipeingreds);
        }

        public virtual List<Recipe> GetRandomSix()
        {
            IQueryable<Recipe> recipes = GetAll();
            return recipes
                .OrderBy(r => Guid.NewGuid())
                .Take(6)
                .ToList();
        }

        public async Task<Recipeingred> CheckRecipeIngreds(Recipeingred recipeIngred)
        {
            if (!_context.Set<Recipeingred>().Contains(recipeIngred))
            {
                recipeIngred.Ingred = CheckIngred(recipeIngred.Ingred);
                return await Task.FromResult(recipeIngred);
            }
            else
            {
                var newRi = _context
                   .Set<Recipeingred>()
                   .Where(i => i.IngredId == recipeIngred.IngredId && i.RecipeId == recipeIngred.RecipeId)
                   .FirstOrDefault();
                newRi.Ingred = CheckIngred(recipeIngred.Ingred);
                return await Task.FromResult(newRi);
            }
        }

        public async Task AddRecipeIngred(Recipeingred recipeingred)
        {
            if (recipeingred == null)
            {
                throw new ArgumentException("Don't pass bad things to me for recipeingredients");
            }
            var recipeIngredDb = _context.Set<Recipeingred>();
            if (recipeIngredDb.Contains(recipeingred))
                recipeIngredDb.Update(recipeingred);
            else
                await recipeIngredDb.AddAsync(recipeingred);
        }

        public Ingredient CheckIngred(Ingredient ingredient)
        {
            if (_context.Set<Ingredient>().Contains(ingredient))
            {
                _context.Set<Ingredient>().Add(ingredient);
                _context.SaveChanges();
            }
            return _context.Set<Ingredient>().Where(i => i.Id == ingredient.Id).FirstOrDefault();
        }

        public virtual async Task SaveListOfRecipes(List<Recipe> recipes)
        {
            foreach (var recipe in recipes)
                if (!GetAll().Any(t => t.Id == recipe.Id))
                {
                    var originalRIs = recipe.Recipeingreds;
                    var ingDb = _context.Set<Ingredient>();
                    var rIngDb = _context.Set<Recipeingred>();
                    recipe.Recipeingreds = null;
                    _dbSet.Add(recipe);
                    await _context.SaveChangesAsync();

                    foreach (var rIng in originalRIs)
                    {
                        if (!rIngDb.Any(r => (r.RecipeId == rIng.RecipeId) && (r.IngredId == rIng.IngredId)))
                        {
                            if (ingDb.Any(i => i.Id == rIng.IngredId))
                            {
                                var grabbed = ingDb.FirstOrDefault(i => i.Id == rIng.IngredId);
                                rIng.Ingred = grabbed;
                            }
                            else
                            {
                                await ingDb.AddAsync(rIng.Ingred);
                            }

                            await rIngDb.AddAsync(rIng);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
        }
    }
}

//var newRecipeIngreds = new List<Recipeingred>();
//foreach (var ri in recipe.Recipeingreds)
//{
//    if (!newRecipeIngreds.Contains(ri))
//        newRecipeIngreds.Add(await CheckRecipeIngreds(ri));
//    else
//        newRecipeIngreds.Where(r => r.RecipeId == ri.RecipeId && r.IngredId == ri.IngredId).FirstOrDefault().Amount += ri.Amount;
//}
//newRecipeIngreds.ForEach(async i => await AddRecipeIngred(i));
//recipe.Recipeingreds = newRecipeIngreds;
//await AddOrUpdateAsync(recipe);
//recipe.UpdateRecipe(recipes.FirstOrDefault());
//    _dbSet.Update(recipe);
//    _context.ChangeTracker.Clear();

//}