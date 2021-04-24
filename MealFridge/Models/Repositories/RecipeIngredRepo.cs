using MealFridge.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Repositories
{
    public class RecipeIngredRepo : Repository<Recipeingred>, IRecipeIngredRepo
    {
        public RecipeIngredRepo(MealFridgeDbContext ctx) : base(ctx) { }

        public List<Recipeingred> GetIngredients(int recipeId)
        {
            return _dbSet.Where(i => i.RecipeId == recipeId).ToList();
        }
    }
}
