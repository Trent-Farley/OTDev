using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Model
{
    public partial class Fridge
    {
        public int? Id { get; set; }
        public string AccountId { get; set; }
        public int IngredId { get; set; }
        public int? Quantity { get; set; }

        public virtual Ingredient Ingred { get; set; }
    }
}
