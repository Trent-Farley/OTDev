using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Models.ViewModels
{
    public class MealFilter
    {
        [Range(0, 14)]
        public int Days { get; set; } = 7;

        [Range(0, 100)]
        public int? Servings { get; set; } = 10;

        [Range(15, 200)]
        public int? Minutes { get; set; } = 200;

        [Range(25, 2000)]
        public double? Calories { get; set; } = 2000;

        [Range(0, 30)]
        public double? TotalFat { get; set; } = 30;

        [Range(0, 30)]
        public double? SatFat { get; set; } = 30;

        [Range(0, 100)]
        public double? Carbs { get; set; } = 100;

        [Range(0, 100)]
        public double? NetCarbs { get; set; } = 100;

        [Range(0, 200)]
        public double? Sugar { get; set; } = 200;

        [Range(0, 100)]
        public double? Cholesterol { get; set; } = 100;

        [Range(0, 2000)]
        public double? Sodium { get; set; } = 2000;

        [Range(0, 200)]
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