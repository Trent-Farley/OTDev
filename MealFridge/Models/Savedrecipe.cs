using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Models
{
    public partial class Savedrecipe
    {
        public string AccountId { get; set; }
        public int RecipeId { get; set; }
        public bool? Shelved { get; set; }
        public bool? Favorited { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
