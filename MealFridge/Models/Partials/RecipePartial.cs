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
    }
}