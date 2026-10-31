using Hotel.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure;
public class HotelContext : DbContext
{
    public HotelContext(DbContextOptions<HotelContext> options) : base(options)
    {
    }

    public DbSet<HotelModel> Hotels => Set<HotelModel>();
    public DbSet<RoomModel> Rooms => Set<RoomModel>();
    public DbSet<StandardRoomModel> StandardRooms => Set<StandardRoomModel>();
    public DbSet<SuiteRoomModel> SuiteRooms => Set<SuiteRoomModel>();
    public DbSet<AmenityModel> Amenities => Set<AmenityModel>();
    public DbSet<GuestModel> Guests => Set<GuestModel>();
    public DbSet<GuestPassportModel> GuestPassports => Set<GuestPassportModel>();
    public DbSet<ReceptionistModel> Receptionists => Set<ReceptionistModel>();
    public DbSet<BookingModel> Bookings => Set<BookingModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------- Hotels ----------
        modelBuilder.Entity<HotelModel>(entity =>
        {
            entity.ToTable("Hotels");
            entity.HasKey(h => h.Id);
            entity.Property(h => h.Name).IsRequired().HasMaxLength(100);
            entity.Property(h => h.City).IsRequired().HasMaxLength(100);

            // ОДИН-ДО-БАГАТЬОХ: Hotel 1 --- * Room
            entity.HasMany(h => h.Rooms)
                  .WithOne(r => r.Hotel)
                  .HasForeignKey(r => r.HotelId)
                  .OnDelete(DeleteBehavior.Cascade);

            // ОДИН-ДО-БАГАТЬОХ: Hotel 1 --- * Receptionist
            entity.HasMany(h => h.Receptionists)
                  .WithOne(r => r.Hotel)
                  .HasForeignKey(r => r.HotelId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- Rooms: Table-per-Type (TPT) ----------
        modelBuilder.Entity<RoomModel>(entity =>
        {
            entity.ToTable("Rooms");
            entity.HasKey(r => r.Id);

            // decimal -> double
            entity.Property(r => r.PricePerNight).HasConversion<double>();

            // номер кімнати унікальний у межах одного готелю
            entity.HasIndex(r => new { r.HotelId, r.RoomNumber }).IsUnique();

            // БАГАТО-ДО-БАГАТЬОХ: Room * --- * Amenity 
            entity.HasMany(r => r.Amenities)
                  .WithMany(a => a.Rooms)
                  .UsingEntity(j => j.ToTable("RoomAmenities"));
        });

        // кожен нащадок -> власна таблиця 
        modelBuilder.Entity<StandardRoomModel>().ToTable("StandardRooms");
        modelBuilder.Entity<SuiteRoomModel>().ToTable("SuiteRooms");

        // ---------- Amenities ----------
        modelBuilder.Entity<AmenityModel>(entity =>
        {
            entity.ToTable("Amenities");
            entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
        });

        // ---------- Guests + Passports ----------
        modelBuilder.Entity<GuestModel>(entity =>
        {
            entity.ToTable("Guests");
            entity.Property(g => g.FullName).IsRequired().HasMaxLength(150);

            // ОДИН-ДО-ОДНОГО: Guest 1 --- 1 GuestPassport 
            entity.HasOne(g => g.Passport)
                  .WithOne(p => p.Guest)
                  .HasForeignKey<GuestPassportModel>(p => p.GuestId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GuestPassportModel>(entity =>
        {
            entity.ToTable("GuestPassports");
            entity.Property(p => p.PassportNumber).IsRequired().HasMaxLength(20);
            entity.HasIndex(p => p.PassportNumber).IsUnique();
        });

        // ---------- Receptionists ----------
        modelBuilder.Entity<ReceptionistModel>(entity =>
        {
            entity.ToTable("Receptionists");
            entity.Property(r => r.FullName).IsRequired().HasMaxLength(150);
        });

        // ---------- Bookings (Guest * --- * Room через окрему сутність) ----------
        modelBuilder.Entity<BookingModel>(entity =>
        {
            entity.ToTable("Bookings");
            entity.Property(b => b.TotalPrice).HasConversion<double>();

            entity.HasOne(b => b.Guest)
                  .WithMany(g => g.Bookings)
                  .HasForeignKey(b => b.GuestId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Room)
                  .WithMany(r => r.Bookings)
                  .HasForeignKey(b => b.RoomId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Receptionist)
                  .WithMany(r => r.ConfirmedBookings)
                  .HasForeignKey(b => b.ReceptionistId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}