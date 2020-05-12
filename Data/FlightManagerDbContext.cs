using FlightManager.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightManager.Data
{
    public class FlightManagerDbContext : IdentityDbContext<Manager>
    {
        public FlightManagerDbContext(DbContextOptions<FlightManagerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
