﻿using TastyMeals.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Models.Repositories
{
    public class IngredientRepo: Repository<Ingredient>, IIngredientRepo
    {
        public IngredientRepo(TastyMealsDbContext ctx) : base(ctx) { }

        public async Task AddIngredAsync(Ingredient ingredient)
        {
            if (ingredient == null)
            {
                throw new ArgumentNullException("Entity must not be null to add or update");
            }
            await _context.AddAsync(ingredient);
            await _context.SaveChangesAsync();
        }

        public List<Ingredient> SearchName(string queryValue)
        {
            return _dbSet.Where(i => i.Name.Contains(queryValue)).ToList();
        }
    }
}
