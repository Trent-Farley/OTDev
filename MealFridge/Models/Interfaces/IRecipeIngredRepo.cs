using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface IRecipeIngredRepo
    {
        List<Recipeingred> GetIngredients(int recipeId);
    }
}
