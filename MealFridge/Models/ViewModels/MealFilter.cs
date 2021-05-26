using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.ViewModels
{
#nullable enable

    public class MealFilter
    {
        [Range(0, 14)]
        public int Days { get; set; } = 7;

        [Range(0, 100)]
        public int? Servings { get; set; } = 10;

        [Range(10, 200)]
        public int? Minutes { get; set; } = 200;

        [Range(25, 2000)]
        public double? Calories { get; set; } = 2000;

        [Range(0, 78)]
        public double? TotalFat { get; set; } = 78;

        [Range(0, 20)]
        public double? SatFat { get; set; } = 20;

        [Range(0, 275)]
        public double? Carbs { get; set; } = 275;

        [Range(0, 275)]
        public double? NetCarbs { get; set; } = 275;

        [Range(0, 200)]     // FDA Recommends capping at 50
        public double? Sugar { get; set; } = 200;

        [Range(0, 300)]
        public double? Cholesterol { get; set; } = 300;

        [Range(0, 2300)]
        public double? Sodium { get; set; } = 2300;

        [Range(0, 200)]  // FDA recommends ~50, but not harmful if more
        public double? Protein { get; set; } = 200;

        public bool Breakfast { get; set; } = true;
        public bool Lunch { get; set; } = true;
        public bool Dinner { get; set; } = true;
        public bool VeryHealthy { get; set; } = false;
        public bool Cheap { get; set; } = false;
        public bool Vegetarian { get; set; } = false;
        public bool Vegan { get; set; } = false;
        public bool GlutenFree { get; set; } = false;
        public bool DairyFree { get; set; } = false;
        public bool Keto { get; set; } = false;
        public bool LactoVeg { get; set; } = false;
        public bool Pescetarian { get; set; } = false;
        public bool Paleo { get; set; } = false;
        public bool Primal { get; set; } = false;
        public bool Whole30 { get; set; } = false;
    }
}