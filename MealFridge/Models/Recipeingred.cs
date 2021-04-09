using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Models
{
    public partial class Recipeingred
    {
        public int RecipeId { get; set; }
        public int IngredId { get; set; }
        public double? Amount { get; set; }
        public string Direction { get; set; }
        public string ServingUnit { get; set; }
        public double? Calories { get; set; }
        public double? TotalFat { get; set; }
        public double? SatFat { get; set; }
        public double? Carbs { get; set; }
        public double? NetCarbs { get; set; }
        public double? Sugar { get; set; }
        public double? Cholesterol { get; set; }
        public double? Sodium { get; set; }
        public double? Protein { get; set; }

        public virtual Ingredient Ingred { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
