using System;
using System.Collections.Generic;

#nullable disable

namespace MealFridge.Models
{
    public partial class Diet
    {
        public string AccountId { get; set; }
        public bool? Vegetarian { get; set; }
        public bool? Vegen { get; set; }
        public bool? GlutenFree { get; set; }
        public bool? DairyFree { get; set; }
        public bool? Keto { get; set; }
        public bool? LactoVeg { get; set; }
        public bool? OvoVeg { get; set; }
        public bool? Pescetarian { get; set; }
        public bool? Paleo { get; set; }
        public bool? Primal { get; set; }
        public bool? Whole30 { get; set; }
    }
}
