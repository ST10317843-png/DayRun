using Microsoft.EntityFrameworkCore;
using DayRun.API.Models;

namespace DayRun.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Venue> Venues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organiser).WithMany()
                .HasForeignKey(e => e.OrganiserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Event).WithMany(e => e.Categories)
                .HasForeignKey(c => c.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue).WithOne(v => v.Event)
                .HasForeignKey<Venue>(v => v.EventId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Enrolment>()
                .HasOne(en => en.Participant).WithMany()
                .HasForeignKey(en => en.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrolment>()
                .HasOne(en => en.Category).WithMany()
                .HasForeignKey(en => en.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrolment>()
                .HasOne(en => en.Result).WithOne(r => r.Enrolment)
                .HasForeignKey<Result>(r => r.EnrolmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrolment>()
                .HasMany(en => en.Payments).WithOne(p => p.Enrolment)
                .HasForeignKey(p => p.EnrolmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Enrolment>().HasIndex(en => new { en.ParticipantId, en.CategoryId }).IsUnique();
            modelBuilder.Entity<Result>().HasIndex(r => r.EnrolmentId).IsUnique();
        }
    }
}