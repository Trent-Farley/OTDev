using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface ISavedrecipeRepo: IRepository<Savedrecipe>
    {
        List<Savedrecipe> FindAccount(string userId, IQueryable<Savedrecipe> other);
        void RemoveSavedRecipe(Savedrecipe recipe);
        Savedrecipe Savedrecipe(string userId, int recipeid);
        List<Savedrecipe> GetShelvedRecipe(string userId, IQueryable<Savedrecipe> other);
        List<Savedrecipe> GetFavoritedRecipe(string userId);
        List<Savedrecipe> GetFavoritedRecipeWithIQueryable(string userId, IQueryable<Savedrecipe> other);
        List<Savedrecipe> GetAllRecipes(string userId, IQueryable<Savedrecipe> other);
    }
}
