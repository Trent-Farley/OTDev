using System;
using MealFridge.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MealFridge.Models;
using MealFridge.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using MealFridge.Utils;


namespace MealFridge.Models.Repositories
{
    public class SavedrecipeRepo : Repository<Savedrecipe>, ISavedrecipeRepo
    {
        public SavedrecipeRepo(MealFridgeDbContext ctx) : base(ctx) { }

      public List<Savedrecipe> FindAccount(string userId)
        {
            var temp = _dbSet.Where(a => a.AccountId == userId).ToList();
            return temp;
        }
        

        public void RemoveSavedRecipe(Savedrecipe recipe)
        {
            _context.Remove(recipe);
            _context.SaveChanges();
        }
        public Savedrecipe Savedrecipe(string userId, int recipeId)
        {
            return _dbSet.Where(r => r.AccountId == userId && r.RecipeId == recipeId).FirstOrDefault();
        }
        public List<Savedrecipe> GetShelvedRecipe(string userId)
        {
            IQueryable<Savedrecipe> temp = GetAll();
            return temp.Where(a => a.AccountId == userId && a.Shelved == true).ToList();
        }
        public List<Savedrecipe> GetFavoritedRecipe(string userId)
        {
            IQueryable<Savedrecipe> temp = GetAll();
            return temp.Where(a => a.AccountId == userId && a.Favorited == true).Include(r => r.Recipe).ToList();
        }
        //public List<Recipe> GetRecipes(int recipeId)
        //{
        //    IQueryable<Recipe> temp = (IQueryable<Recipe>)GetAll();
        //    return temp.ToList();
        //}
    }
}

        