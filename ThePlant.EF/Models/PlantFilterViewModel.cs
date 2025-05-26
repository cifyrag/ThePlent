using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePlant.EF.Models
{
    public class PlantFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string? CurrentCategory { get; set; }
        public int? CurrentCareFrequency { get; set; }
        public List<string>? Categories { get; set; }
    }


}
