using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePlant.EF.Models
{
    public class PlantCareInstruction
    {
        [Key]
        public Guid PlantCareInstructionId { get; set; }= Guid.NewGuid(); 

        [Required]
        public Guid PlantId { get; set; } 

        [Required]
        public string? Description { get; set; }

        public int FrequencyRecommended { get; set; }

        public string? Note { get; set; }

        [ForeignKey("PlantId")]
        public virtual Plant? Plant { get; set; }

    }

}
