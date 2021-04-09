using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Models
{
    public partial class Fridge
    {
        public int? Id { get; set; }
        public string AccountId { get; set; }
        public int IngredId { get; set; }
        public double? Quantity { get; set; }
        public bool? Shopping { get; set; }
        public double? NeededAmount { get; set; }

        public virtual Ingredient Ingred { get; set; }
    }
}
