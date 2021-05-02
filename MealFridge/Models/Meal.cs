using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Models
{
    public partial class Meal
    {
        public string AccountId { get; set; }
        public int? RecipeId { get; set; }
        public string Type { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
