using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MealFridge.Models
{
    [Table("RECIPEINGRED")]
    public partial class Recipeingred
    {
        [Key]
        [Column("recipe_id")]
        public int RecipeId { get; set; }
        [Key]
        [Column("ingred_id")]
        public int IngredId { get; set; }
        [Column("amount")]
        [StringLength(255)]
        public string Amount { get; set; }
        [Column("step")]
        public int? Step { get; set; }
        [Column("direction")]
        [StringLength(255)]
        public string Direction { get; set; }

        [ForeignKey(nameof(IngredId))]
        [InverseProperty(nameof(Ingredient.Recipeingreds))]
        public virtual Ingredient Ingred { get; set; }
        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("Recipeingreds")]
        public virtual Recipe Recipe { get; set; }
    }
}
