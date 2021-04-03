using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealFridge.Models
{
    /// <summary>
    /// This class will be used in the Ingredient search for the fridge. This class 
    /// will hold the normal ingredient items (Name, image) and how many are currently in
    /// their inventory. 
    /// </summary>
    public class IngredientInventory
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
    }
}
