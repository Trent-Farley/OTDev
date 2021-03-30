using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Models
{
    public partial class Recipeingred
    {
        public int RecipeId { get; set; }
        public int IngredId { get; set; }
        public string Amount { get; set; }
        public int? Step { get; set; }
        public string Direction { get; set; }

        public virtual Ingredient Ingred { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
