using FlightManager.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FlightManager.Data
{
    public class FlightManagerDbContext : IdentityDbContext<Manager>, IDbContext
    {
        public FlightManagerDbContext(DbContextOptions<FlightManagerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }

        public virtual DbSet<City> Cities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Flight>()
            .HasOne(f => f.Origin)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Flight>()
            .HasOne(f => f.Destination)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        }

        IQueryable<T> IDbContext.Set<T>()
        {
            return base.Set<T>();
        }
    }
}
