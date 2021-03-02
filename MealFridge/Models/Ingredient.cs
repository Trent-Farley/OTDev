using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MealFridge.Models
{
    [Table("INGREDIENTS")]
    public partial class Ingredient
    {
        public Ingredient()
        {
            Fridges = new HashSet<Fridge>();
            Recipeingreds = new HashSet<Recipeingred>();
            Restrictions = new HashSet<Restriction>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }
        [Column("aisle")]
        [StringLength(255)]
        public string Aisle { get; set; }
        [Column("cost", TypeName = "money")]
        public decimal? Cost { get; set; }

        [InverseProperty(nameof(Fridge.Ingred))]
        public virtual ICollection<Fridge> Fridges { get; set; }
        [InverseProperty(nameof(Recipeingred.Ingred))]
        public virtual ICollection<Recipeingred> Recipeingreds { get; set; }
        [InverseProperty(nameof(Restriction.Ingred))]
        public virtual ICollection<Restriction> Restrictions { get; set; }
    }
}
