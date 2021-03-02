using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HimalayanExpeditions.Models
{
    public class SearchClimbers
    {
        public int? Age { get; set; }
        public string Name { get; set; }
        public IEnumerable<Climber> ClimberList { get; set; }

    }
}
