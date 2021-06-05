using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Models.Interfaces
{
    public interface ISavedrecipeRepo: IRepository<SavedRecipe>
    {
        List<SavedRecipe> FindAccount(string userId, IQueryable<SavedRecipe> other);
        void RemoveSavedRecipe(SavedRecipe recipe);
        SavedRecipe Savedrecipe(string userId, int recipeid);
        SavedRecipe CreateNewSavedRecipe(Recipe recipe, string userId);
        List<SavedRecipe> GetShelvedRecipe(string userId, IQueryable<SavedRecipe> other);
        List<SavedRecipe> GetFavoritedRecipe(string userId);
        List<SavedRecipe> GetFavoritedRecipeWithIQueryable(string userId, IQueryable<SavedRecipe> other);
        List<SavedRecipe> GetAllRecipes(string userId, IQueryable<SavedRecipe> other);
    }
}
