using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePlant.EF.Models
{
    public class Plant
    {
        [Key]
        public Guid PlantId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(150)]
        public string? PlantName { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(200)]
        public string? ScientificTitle { get; set; }

        public virtual ICollection<UserPlant>? UserPlants { get; set; }
        public virtual ICollection<PlantCareInstruction>? PlantCareInstructions { get; set; }
        public virtual ICollection<PlantOverview>? PlantOverviews { get; set; }
        public virtual ICollection<PlantImage>? PlantImages { get; set; }

    }
}
