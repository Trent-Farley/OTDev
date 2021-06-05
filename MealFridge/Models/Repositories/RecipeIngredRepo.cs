using TastyMeals.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Models.Repositories
{
    public class RecipeIngredRepo : Repository<RecipeIngredient>, IRecipeIngredRepo
    {
        public RecipeIngredRepo(TastyMealsDbContext ctx) : base(ctx) { }

        public List<RecipeIngredient> GetIngredients(int recipeId)
        {
            return _dbSet.Where(i => i.RecipeId == recipeId).ToList();
        }
    }
}
