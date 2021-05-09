using MealFridge.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models
{
    public partial class Recipe : IFoodItem
    {
        [NotMapped]
        public bool Dislike { get; set; }

        [NotMapped]
        public bool Banned { get; set; }

        public string GetRecipeType()
        {
            if (Breakfast == true)
                return "Breakfast";
            else if (Lunch == true)
                return "Lunch";
            else if (Dinner == true)
                return "Dinner";
            else if (Snack == true)
                return "Snack";
            return "";
        }
    }
}