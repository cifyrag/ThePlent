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
    public class Reminder
    {
        [Key]
        public Guid ReminderId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserPlantId { get; set; }

        public DateTime DateOfReminder { get; set; }

        public ReminderType ReminderType { get; set; } 

        [MaxLength(50)]
        public string? Frequency { get; set; } 

        public ReminderStatus Status { get; set; }

        [MaxLength(100)]
        public string? CompletionType { get; set; } 

        public DateTime? PreviousDate { get; set; }

        [ForeignKey("UserPlantId")]
        public virtual UserPlant? UserPlant { get; set; }

    }

}
