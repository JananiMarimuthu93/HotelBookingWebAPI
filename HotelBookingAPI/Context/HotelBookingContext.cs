using HotelBookingAPI.Models.AuthModels;
using HotelBookingAPI.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelBookingAPI.Context
{
    public class HotelBookingContext : DbContext
    {
        public HotelBookingContext() { }
        public HotelBookingContext(DbContextOptions<HotelBookingContext> options) : base(options) 
        {

        }

        // Auth Models
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        
        // DbSets
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ENUMS stored as string
            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasConversion<string>();


            // RELATIONSHIPS
            modelBuilder.Entity<RoomType>()
                .HasMany(rt => rt.Rooms)
                .WithOne(r => r.RoomType)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Bookings)
                .WithOne(b => b.Room)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Guest>()
                .HasMany(g => g.Bookings)
                .WithOne(b => b.Guest)
                .HasForeignKey(b => b.GuestId)
                .OnDelete(DeleteBehavior.Cascade);

            DateTime seedDate1 = new DateTime(2025, 08, 24, 10, 0, 0);
            DateTime seedDate2 = new DateTime(2025, 08, 25, 12, 0, 0);
            DateTime seedDate3 = new DateTime(2025, 08, 26, 14, 0, 0);

            // RoomTypes
            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { RoomTypeId = 1, TypeName = "Single", Description = "Single bed room", IsActive = true, CreatedAt = seedDate1 },
                new RoomType { RoomTypeId = 2, TypeName = "Double", Description = "Double bed room", IsActive = true, CreatedAt = seedDate1 },
                new RoomType { RoomTypeId = 3, TypeName = "Suite", Description = "Luxury suite", IsActive = true, CreatedAt = seedDate1 }
            );

            // Rooms
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 1, RoomNumber = "101", Capacity = 1, PricePerDay = 2000, IsAvailable = true, RoomTypeId = 1, CreatedAt = seedDate1 },
                new Room { RoomId = 2, RoomNumber = "102", Capacity = 2, PricePerDay = 3500, IsAvailable = true, RoomTypeId = 2, CreatedAt = seedDate1 },
                new Room { RoomId = 3, RoomNumber = "201", Capacity = 4, PricePerDay = 8000, IsAvailable = true, RoomTypeId = 3, CreatedAt = seedDate1 }
            );

            // Guests
            modelBuilder.Entity<Guest>().HasData(
                new Guest { GuestId = 1, FullName = "Janani M", Email = "jan@gmail.com", Phone = "9385562091", Address = "123 Street, City", CreatedAt = seedDate2 },
                new Guest { GuestId = 2, FullName = "Deepika M", Email = "dep@gmail.com", Phone = "9876543211", Address = "456 Avenue, City", CreatedAt = seedDate2 }
            );

            // Bookings
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = 1,
                    CheckInDate = seedDate2,
                    CheckOutDate = seedDate3,
                    NumberOfGuests = 1,
                    TotalAmount = 4000,
                    Status = BookingStatus.Confirmed,
                    CreatedAt = seedDate1,
                    GuestId = 1,
                    RoomId = 1
                },
                new Booking
                {
                    BookingId = 2,
                    CheckInDate = seedDate3,
                    CheckOutDate = seedDate3.AddDays(2),
                    NumberOfGuests = 2,
                    TotalAmount = 7000,
                    Status = BookingStatus.Pending,
                    CreatedAt = seedDate1,
                    GuestId = 2,
                    RoomId = 2
                }
            );

            
        }
    }
}
