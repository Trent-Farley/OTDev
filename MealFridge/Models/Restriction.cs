using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Models
{
    public partial class Restriction
    {
        public string AccountId { get; set; }
        public int IngredId { get; set; }
        public bool? Dislike { get; set; }


        public virtual Ingredient Ingred { get; set; }
    }
}
