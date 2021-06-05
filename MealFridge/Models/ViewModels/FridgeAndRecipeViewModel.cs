﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TastyMeals.Models.Repositories;

namespace TastyMeals.Models
{
    public class FridgeAndRecipeViewModel
    {
        public List<Fridge> GetFridge { get; set; }
        public Recipe GetRecipes { get; set; }
        public List<Ingredient> GetIngredients { get; set; }

        public string StringIt()
        {
            var s = "";
            foreach(var i in GetIngredients)
            {
                s += i.Id + " ";
            }
            return s;
        }
    }
}
