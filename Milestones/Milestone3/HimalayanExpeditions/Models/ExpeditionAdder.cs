using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HimalayanExpeditions.Models
{
    public class ExpeditionAdder 
    {
        public Expedition Expedition { get; set; } 
        public bool OxyCheck { get; set; }
        public IEnumerable<Expedition> Expeditions { get; set; }
        public IEnumerable<Peak> Peaks { get; set; }
        public IEnumerable<TrekkingAgency> TrekkingAgencies { get; set; }
    }
}
