using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HimalayanExpeditions.Models
{
    [Table("Expedition")]
    public partial class Expedition
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(10)]
        public string Season { get; set; }
        public int? Year { get; set; }
        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }
        [StringLength(80)]
        public string TerminationReason { get; set; }
        public bool? OxygenUsed { get; set; }
        [Column("PeakID")]
        public int? PeakId { get; set; }
        [Column("TrekkingAgencyID")]
        public int? TrekkingAgencyId { get; set; }

        [ForeignKey(nameof(PeakId))]
        [InverseProperty("Expeditions")]
        public virtual Peak Peak { get; set; }
        [ForeignKey(nameof(TrekkingAgencyId))]
        [InverseProperty("Expeditions")]
        public virtual TrekkingAgency TrekkingAgency { get; set; }
    }
}
