using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HimalayanExpeditions.Models
{
    [Table("Climber")]
    public partial class Climber
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(60)]
        public string Name { get; set; }
        [Column("AGE")]
        public int? Age { get; set; }
    }
}
