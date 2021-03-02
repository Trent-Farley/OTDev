using System;
using System.Collections.Generic;


#nullable disable

namespace MealFridge.Models
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            Fridges = new HashSet<Fridge>();
            Recipeingreds = new HashSet<Recipeingred>();
            Restrictions = new HashSet<Restriction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Aisle { get; set; }
        public decimal? Cost { get; set; }

        public virtual ICollection<Fridge> Fridges { get; set; }
        public virtual ICollection<Recipeingred> Recipeingreds { get; set; }

        public virtual ICollection<Restriction> Restrictions { get; set; }
    }
}
