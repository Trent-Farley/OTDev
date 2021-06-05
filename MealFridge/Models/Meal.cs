using System;
using System.Collections.Generic;

#nullable disable

namespace TastyMeals.Models
{
    public partial class Meal
    {
        public string AccountId { get; set; }
        public DateTime Day { get; set; }
        public int? RecipeId { get; set; }
        public string MealType { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
