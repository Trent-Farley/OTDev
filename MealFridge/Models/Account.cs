using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace MealFridge.Models
{
    [Table("ACCOUNT")]
    public partial class Account
    {
        public Account()
        {
            Fridges = new HashSet<Fridge>();
            Meals = new HashSet<Meal>();
            Restrictions = new HashSet<Restriction>();
            Savedrecipes = new HashSet<Savedrecipe>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("fridge_id")]
        public int? FridgeId { get; set; }

        [InverseProperty(nameof(Fridge.FridgeNavigation))]
        public virtual ICollection<Fridge> Fridges { get; set; }
        [InverseProperty(nameof(Meal.Account))]
        public virtual ICollection<Meal> Meals { get; set; }
        [InverseProperty(nameof(Restriction.Account))]
        public virtual ICollection<Restriction> Restrictions { get; set; }
        [InverseProperty(nameof(Savedrecipe.Account))]
        public virtual ICollection<Savedrecipe> Savedrecipes { get; set; }
    }
}
