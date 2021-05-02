using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealFridge.Models;
using MealFridge.Models.Interfaces;
using MealFridge.Models.ViewModels;

namespace MealFridge.Models.Repositories
{
    public class RecipeRepo : Repository<Recipe>, IRecipeRepo
    {
        public RecipeRepo(MealFridgeDbContext ctx) : base(ctx)
        {
        }

        public virtual List<Recipe> GetRandomSix()
        {
            IQueryable<Recipe> recipes = GetAll();
            return recipes
                .OrderBy(r => Guid.NewGuid())
                .Take(6)
                .ToList();
        }

        public async Task SaveDetails(Recipe recipe)
        {
            _dbSet.Update(recipe);
            foreach (var ingred in recipe.Recipeingreds)
            {
                if (!_context.Set<Recipeingred>().Any(r => (r.RecipeId == ingred.RecipeId) && (r.IngredId == ingred.IngredId)))
                {
                    if (_context.Set<Ingredient>().Any(i => i.Id == ingred.IngredId))
                    {
                        ingred.Ingred = _context.Set<Ingredient>().FirstOrDefault(i => i.Id == ingred.IngredId);
                    }
                    else
                    {
                        await _context.Set<Ingredient>().AddAsync(ingred.Ingred);
                    }
                    await _context.Set<Recipeingred>().AddAsync(ingred);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}