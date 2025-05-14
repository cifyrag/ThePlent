using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePlant.EF.Models.Enam;

namespace ThePlant.EF.Models
{
    public class PlantOverview
    {
        [Key]
        public Guid PlantOverviewId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PlantId { get; set; } 

        public OverviewType OverviewType { get; set; } 

        [Required]
        public string? Description { get; set; }

        [ForeignKey("PlantId")]
        public virtual Plant? Plant { get; set; }
    }
}
