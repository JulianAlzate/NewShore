using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace API.Model
{
    public partial class NewShoreContext : DbContext
    {
        public NewShoreContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<NewShoreContext>();
            var connectionString = configuration.GetConnectionString("NewShoreConnection");
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
        public NewShoreContext(DbContextOptions<NewShoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Journey> Journeys { get; set; }
        public virtual DbSet<JourneyFlight> JourneyFlights { get; set; }
        public virtual DbSet<Transport> Transports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.HasKey(e => e.IdFlight);

                entity.ToTable("Flight");

                entity.Property(e => e.Destination)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("destination");

                entity.Property(e => e.Origin)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("origin");

                entity.Property(e => e.Price)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("price");

                entity.HasOne(d => d.IdTransportNavigation)
                    .WithMany(p => p.Flights)
                    .HasForeignKey(d => d.IdTransport)
                    .HasConstraintName("FK_Flight_Transport");
            });

            modelBuilder.Entity<Journey>(entity =>
            {
                entity.HasKey(e => e.IdJourney);

                entity.ToTable("Journey");

                entity.Property(e => e.Destination)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("destination");

                entity.Property(e => e.Origin)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("origin");

                entity.Property(e => e.Price)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("price");
            });

            modelBuilder.Entity<JourneyFlight>(entity =>
            {
                entity.HasKey(e => e.IdJourneyFlight);

                entity.ToTable("JourneyFlight");

                entity.Property(e => e.IdJourneyFlight).HasColumnName("idJourneyFlight");

                entity.HasOne(d => d.IdFlightNavigation)
                    .WithMany(p => p.JourneyFlights)
                    .HasForeignKey(d => d.IdFlight)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JourneyFlight_Flight");

                entity.HasOne(d => d.IdJourneyNavigation)
                    .WithMany(p => p.JourneyFlights)
                    .HasForeignKey(d => d.IdJourney)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JourneyFlight_Journey");
            });

            modelBuilder.Entity<Transport>(entity =>
            {
                entity.HasKey(e => e.IdTransport);

                entity.ToTable("Transport");

                entity.Property(e => e.FlightCarrier)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("flightCarrier");

                entity.Property(e => e.FlightNumber).HasColumnName("flightNumber");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
