using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ThePlant.EF.Models;

namespace ThePlant.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<UserPlant> UserPlants { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<PlantCareInstruction> PlantCareInstructions { get; set; }
        public DbSet<PlantOverview> PlantOverviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSubscriptions)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.Subscription)
                .WithMany(s => s.UserSubscriptions)
                .HasForeignKey(us => us.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAchievements)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.Achievement)
                .WithMany(a => a.UserAchievements)
                .HasForeignKey(ua => ua.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPlant>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPlants)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPlant>()
                .HasOne(up => up.Plant)
                .WithMany(p => p.UserPlants)
                .HasForeignKey(up => up.PlantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.UserPlant)
                .WithMany(up => up.Reminders)
                .HasForeignKey(r => r.UserPlantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlantCareInstruction>()
                .HasOne(pci => pci.Plant)
                .WithMany(p => p.PlantCareInstructions)
                .HasForeignKey(pci => pci.PlantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlantOverview>()
                .HasOne(po => po.Plant)
                .WithMany(p => p.PlantOverviews)
                .HasForeignKey(po => po.PlantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }


}
