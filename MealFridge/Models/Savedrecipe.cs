using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MealFridge.Models
{
    [Table("SAVEDRECIPES")]
    public partial class Savedrecipe
    {
        [Key]
        [Column("account_id")]
        public int AccountId { get; set; }
        [Key]
        [Column("recipe_id")]
        public int RecipeId { get; set; }
        [Column("shelved")]
        public bool? Shelved { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("Savedrecipes")]
        public virtual Account Account { get; set; }
        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("Savedrecipes")]
        public virtual Recipe Recipe { get; set; }
    }
}
