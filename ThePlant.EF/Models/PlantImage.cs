using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThePlant.EF.Models
{
    public class PlantImage
    {
        [Key]
        public Guid PlantImageId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PlantId { get; set; }

        [Required]
        [MaxLength(500)] 
        public string URL { get; set; }

        [ForeignKey(nameof(PlantId))]
        public virtual Plant Plant { get; set; }
    }
}
