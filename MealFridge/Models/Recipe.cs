using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MealFridge.Models
{
    [Table("RECIPES")]
    public partial class Recipe
    {
        public Recipe()
        {
            Meals = new HashSet<Meal>();
            Recipeingreds = new HashSet<Recipeingred>();
            Savedrecipes = new HashSet<Savedrecipe>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        [StringLength(255)]
        public string Title { get; set; }
        [Column("image")]
        [StringLength(255)]
        public string Image { get; set; }
        [Column("servings")]
        public int? Servings { get; set; }
        [Column("minutes")]
        public int? Minutes { get; set; }
        [Column("summery")]
        [StringLength(255)]
        public string Summery { get; set; }
        [Column("instructions")]
        [StringLength(255)]
        public string Instructions { get; set; }

        [InverseProperty(nameof(Meal.Recipe))]
        public virtual ICollection<Meal> Meals { get; set; }
        [InverseProperty(nameof(Recipeingred.Recipe))]
        public virtual ICollection<Recipeingred> Recipeingreds { get; set; }
        [InverseProperty(nameof(Savedrecipe.Recipe))]
        public virtual ICollection<Savedrecipe> Savedrecipes { get; set; }
    }
}
