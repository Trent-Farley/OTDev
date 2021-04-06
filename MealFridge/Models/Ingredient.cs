using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Model
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
        public string Image { get; set; }
        public string Name { get; set; }
        public string Aisle { get; set; }
        public decimal? Cost { get; set; }
        public double? ServingSize { get; set; }
        public string ServingUnit { get; set; }
        public double? Calories { get; set; }
        public double? TotalFat { get; set; }
        public double? SatFat { get; set; }
        public double? Carbs { get; set; }
        public double? NetCarbs { get; set; }
        public double? Sugar { get; set; }
        public double? Cholesterol { get; set; }
        public double? Sodium { get; set; }
        public double? Protein { get; set; }

        public virtual ICollection<Fridge> Fridges { get; set; }
        public virtual ICollection<Recipeingred> Recipeingreds { get; set; }
        public virtual ICollection<Restriction> Restrictions { get; set; }
    }
}
