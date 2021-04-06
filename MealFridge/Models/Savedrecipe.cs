using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Model
{
    public partial class Savedrecipe
    {
        public string AccountId { get; set; }
        public int RecipeId { get; set; }
        public bool? Shelved { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
