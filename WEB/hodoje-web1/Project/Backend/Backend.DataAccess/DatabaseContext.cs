using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities.Models;

namespace Backend.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOptional(u => u.Car)
                .WithRequired(c => c.Driver);
            modelBuilder.Entity<User>()
                .HasOptional(u => u.DriverLocation)
                .WithMany(l => l.Drivers)
                .HasForeignKey(u => u.DriverLocationId);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithRequired(c => c.User)
                .HasForeignKey(c => c.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ride>()
                .Property(r => r.Timestamp)
                .HasColumnType("datetime2");
            modelBuilder.Entity<Ride>()
                .HasRequired(r => r.StartLocation)
                .WithMany(l => l.RideStarts)
                .HasForeignKey(r => r.StartLocationId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Ride>() 
                .HasOptional(r => r.DestinationLocation)            // Actually not optional, but because of initial state where it is not defined, it is optional
                .WithMany(l => l.RideDestinations)
                .HasForeignKey(r => r.DestinationLocationId);
            modelBuilder.Entity<Ride>()
                .HasOptional(r => r.Customer)
                .WithMany(c => c.CustomerRides)
                .HasForeignKey(r => r.CustomerId);
            modelBuilder.Entity<Ride>()
                .HasOptional(r => r.Dispatcher)
                .WithMany(d => d.DispatcherRides)
                .HasForeignKey(r => r.DispatcherId);
            modelBuilder.Entity<Ride>()
                .HasOptional(r => r.Driver)                         // Actually not optional, but because of initial state where it is not defined, it is optional
                .WithMany(d => d.DriverRides)
                .HasForeignKey(r => r.DriverId);
            modelBuilder.Entity<Ride>()
                .HasMany(r => r.Comments)
                .WithRequired(c => c.Ride)
                .HasForeignKey(c => c.RideId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .Property(c => c.Timestamp)
                .HasColumnType("datetime2");
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Ride> Rides { get; set; }
    }
}
