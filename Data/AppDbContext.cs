using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Models.Entities;

namespace Server.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<string>, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ProductPresentation> ProductPresentations { get; set; }
        public DbSet<ProductSlide> ProductSlides { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Appointment>()
                .HasOne(a => a.Location)
                .WithMany()
                .HasForeignKey(a => a.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductPresentation>()
                .HasOne(p => p.Appointment)
                .WithMany(a => a.ProductPresentations)
                .HasForeignKey(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductPresentation>()
                .HasMany(p => p.ProductSlides)
                .WithOne(s => s.ProductPresentation)
                .HasForeignKey(s => s.ProductPresentationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductSlide>()
                .Property(s => s.Feedback)
                .HasConversion<int>(); // store enum as integer
        }

        // public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        // {
        //     foreach (var entry in ChangeTracker.Entries<ProductSlide>())
        //     {
        //         if (entry.State == EntityState.Modified)
        //         {
        //             entry.Entity.UpdatedAt = DateTime.UtcNow;
        //         }
        //     }
        //     return base.SaveChangesAsync(cancellationToken);
        // }
    }
}