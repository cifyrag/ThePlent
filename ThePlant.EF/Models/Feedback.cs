using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePlant.EF.Models
{
    public class Feedback
    {
        [Key]
        public Guid FeedbackId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; } 

        [MaxLength(200)]
        public string? Theme { get; set; }

        [Required]
        public string? Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

    }

}
