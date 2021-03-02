using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Models
{
    public partial class Recipe
    {
        public Recipe()
        {
            Meals = new HashSet<Meal>();
            Recipeingreds = new HashSet<Recipeingred>();
            Savedrecipes = new HashSet<Savedrecipe>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int? Servings { get; set; }
        public int? Minutes { get; set; }
        public string Summery { get; set; }
        public string Instructions { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }
        public virtual ICollection<Recipeingred> Recipeingreds { get; set; }

        public virtual ICollection<Savedrecipe> Savedrecipes { get; set; }
    }
}
