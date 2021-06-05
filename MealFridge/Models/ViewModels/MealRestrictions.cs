using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TastyMeals.Models.ViewModels
{

    public class MealRestrictions
    {
        public Meals Meals { get; set; }
        public MealFilter MealFilter { get; set; } = new MealFilter();
        public bool Tried { get; set; }
    }
}