using System;
using System.Collections.Generic;

#nullable disable

namespace TastyMeals.Models
{
    public partial class Fridge
    {
        public string AccountId { get; set; }
        public int IngredId { get; set; }
        public string UnitType { get; set; }
        public double? Quantity { get; set; }
        public bool? Shopping { get; set; }
        public double? NeededAmount { get; set; }

        public virtual Ingredient Ingred { get; set; }
    }
}
