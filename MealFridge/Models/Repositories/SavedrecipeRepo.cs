using System;
using TastyMeals.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TastyMeals.Models;
using TastyMeals.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using TastyMeals.Utils;


namespace TastyMeals.Models.Repositories
{
    public class SavedrecipeRepo : Repository<SavedRecipe>, ISavedrecipeRepo
    {
        public SavedrecipeRepo(TastyMealsDbContext ctx) : base(ctx) { }

        public List<SavedRecipe> FindAccount(string userId, IQueryable<SavedRecipe> other)
        {
            var temp = other.Where(a => a.AccountId == userId).ToList();
            return temp;
        }
        

        public void RemoveSavedRecipe(SavedRecipe recipe)
        {
            _context.Remove(recipe);
            _context.SaveChanges();
        }
        public SavedRecipe Savedrecipe(string userId, int recipeId)
        {
            return _dbSet.Where(r => r.AccountId == userId && r.RecipeId == recipeId).FirstOrDefault();
        }
        public List<SavedRecipe> GetShelvedRecipe(string userId, IQueryable<SavedRecipe> other)
        {
            return other.Where(a => a.AccountId == userId && a.Shelved == true).ToList();
        }
       
        //Should not touch/refactor this one for testing, it is being used for other things 
        public List<SavedRecipe> GetFavoritedRecipe(string userId)
        {
            IQueryable<SavedRecipe> other = GetAll();
            return other.Where(a => a.AccountId == userId && a.Favorited == true).Include(r => r.Recipe).ToList();
        }

        public List<SavedRecipe> GetFavoritedRecipeWithIQueryable(string userId, IQueryable<SavedRecipe> other)
        {
            return other.Where(a => a.AccountId == userId && a.Favorited == true).ToList();
        }
        public List<SavedRecipe> GetAllRecipes(string userId, IQueryable<SavedRecipe> other)
        {
            return other.Where(a => a.AccountId == userId).Include(r => r.Recipe).ToList();
        }

        public SavedRecipe CreateNewSavedRecipe(Recipe recipe, string userId)
        {
            return new SavedRecipe { AccountId = userId, Recipe = recipe, RecipeId = recipe.Id, Favorited = null, Shelved = null };
        }
       
    }
}

        