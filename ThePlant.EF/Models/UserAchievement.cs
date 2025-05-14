using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePlant.EF.Models
{
    public class UserAchievement
    {
        [Key]
        public Guid UserAchievementId { get; set; }  = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid AchievementId { get; set; }

        public int UserProgress { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("AchievementId")]
        public virtual Achievement? Achievement { get; set; }

    }

}
