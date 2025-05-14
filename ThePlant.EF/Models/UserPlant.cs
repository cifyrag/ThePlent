using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePlant.EF.Models
{
    public class UserPlant
    {
        [Key]
        public Guid UserPlantId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid PlantId { get; set; }

        [MaxLength(150)]
        public string? UserPlantName { get; set; } 

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("PlantId")]
        public virtual Plant? Plant { get; set; }

        public virtual ICollection<Reminder>? Reminders { get; set; }

    }

}
