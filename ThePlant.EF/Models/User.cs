using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePlant.EF.Models.Enam;

namespace ThePlant.EF.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(250)]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? TimeZone { get; set; }

        public string? Location { get; set; } 

        public Language Lang { get; set; }

        public bool IsAdmin { get; set; }

        [Required]
        public string? Password { get; set; }

        public bool AllowsNotifications { get; set; } = false;

        public virtual ICollection<Feedback>? Feedbacks { get; set; }
        public virtual ICollection<UserSubscription>? UserSubscriptions { get; set; }
        public virtual ICollection<UserAchievement>? UserAchievements { get; set; }
        public virtual ICollection<UserPlant>? UserPlants { get; set; }
        public virtual ICollection<Reminder>? Reminders { get; set; }
    }
}
