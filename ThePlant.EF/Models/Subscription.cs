using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePlant.EF.Models
{
    public class Subscription
    {
        [Key]
        public Guid SubscriptionId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string? Description { get; set; }

        public float Price { get; set; } 

        [MaxLength(50)]
        public string? Duration { get; set; } 

        public virtual ICollection<UserSubscription>? UserSubscriptions { get; set; }

    }
}
