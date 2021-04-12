using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealFridge.Models.Interfaces
{
    public interface IFoodItem
    {
        public double? Calories { get; set; }
        public double? TotalFat { get; set; }
        public double? SatFat { get; set; }
        public double? Carbs { get; set; }
        public double? NetCarbs { get; set; }
        public double? Sugar { get; set; }
        public double? Cholesterol { get; set; }
        public double? Sodium { get; set; }
        public double? Protein { get; set; }
    }
}