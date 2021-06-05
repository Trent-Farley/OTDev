using System;
using System.Collections.Generic;

#nullable disable

namespace TastyMeals.Models
{
    public partial class Restriction
    {
        public string AccountId { get; set; }
        public int IngredId { get; set; }
        public bool? Dislike { get; set; }
        public bool? Banned { get; set; }

        public virtual Ingredient Ingred { get; set; }
    }
}
