using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePlant.EF.Models
{
    public class Achievement
    {
        [Key]
        public Guid AchievementId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string? Description { get; set; }

        public int ProgressGoal { get; set; } 

        public virtual ICollection<UserAchievement>? UserAchievements { get; set; }
    }
}
