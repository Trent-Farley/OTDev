﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealFridge.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MealFridge.Models.Repositories
{
    public class RecipeRepo : Repository<Recipe>, IRecipeRepo
    {
        public RecipeRepo(MealFridgeDbContext ctx) : base(ctx)
        {
            _dbSet.Include(ri => ri.Recipeingreds);
        }

        public virtual List<Recipe> GetRandomSix()
        {
            IQueryable<Recipe> recipes = GetAll();
            return recipes
                .OrderBy(r => Guid.NewGuid())
                .Take(6)
                .ToList();
        }

        public virtual async Task SaveListOfRecipes(List<Recipe> recipes)
        {
            foreach (var recipe in recipes)
                if (!GetAll().Any(t => t.Id == recipe.Id))
                {
                    var originalRIs = recipe.Recipeingreds;
                    var ingDb = _context.Set<Ingredient>();
                    var rIngDb = _context.Set<Recipeingred>();
                    recipe.Recipeingreds = null;
                    _dbSet.Add(recipe);
                    await _context.SaveChangesAsync();
                    foreach (var rIng in originalRIs)
                    {
                        if (!rIngDb.Any(r => (r.RecipeId == rIng.RecipeId) && (r.IngredId == rIng.IngredId)))
                        {
                            if (ingDb.Any(i => i.Id == rIng.IngredId))
                            {
                                var grabbed = ingDb.FirstOrDefault(i => i.Id == rIng.IngredId);
                                rIng.Ingred = grabbed;
                            }
                            else
                                await ingDb.AddAsync(rIng.Ingred);

                            await rIngDb.AddAsync(rIng);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
        }
    }
}