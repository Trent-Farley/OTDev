using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MealFridge.Models
{
    [Table("MEAL")]
    public partial class Meal
    {
        [Key]
        [Column("account_id")]
        public int AccountId { get; set; }
        [Key]
        [Column("day", TypeName = "datetime")]
        public DateTime Day { get; set; }
        [Column("recipe_id")]
        public int? RecipeId { get; set; }
        [Column("meal")]
        [StringLength(255)]
        public string Meal1 { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("Meals")]
        public virtual Account Account { get; set; }
        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("Meals")]
        public virtual Recipe Recipe { get; set; }
    }
}
