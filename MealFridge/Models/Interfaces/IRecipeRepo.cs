﻿using TastyMeals.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Models.Interfaces
{
    public interface IRecipeRepo : IRepository<Recipe>
    {
        public List<Recipe> GetRandomSix();

        public Task SaveListOfRecipes(List<Recipe> recipes);

        public List<Recipe> GetRecipesbyNames(List<string> recipes, IQueryable<Recipe> recipeRepo);

        public IQueryable<Recipe> GetRecipesByName(string name);
    }
}