using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Services
{
    public class BookingService
    {
        private readonly IGenericRepository<Booking> _genericRepo;
        private readonly IGenericRepository<Room> _roomRepo;

        public BookingService(IGenericRepository<Booking> genericRepo, IGenericRepository<Room> roomRepo)
        {
            _genericRepo = genericRepo;
            _roomRepo = roomRepo;
        }

        // Get all bookings
        public async Task<IEnumerable<BookingReadDto>> GetAllAsync()
        {
            var bookings = await _genericRepo.GetAllQueryable()
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .AsNoTracking()
                .ToListAsync();

            return bookings.Select(MapToReadDto);
        }

        // Get booking by ID
        public async Task<BookingReadDto?> GetByIdAsync(int id)
        {
            var booking = await _genericRepo.GetAllQueryable()
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return null;

            return MapToReadDto(booking);
        }

        // Create new booking
        public async Task<BookingReadDto> CreateAsync(BookingCreateDto dto)
        {
            var today = DateTime.UtcNow.Date;

            //  Validation: Check-in must be today or in the future
            if (dto.CheckInDate.Date < today)
                throw new Exception("Check-in date cannot be in the past.");

            //  Validation: Check-out must be after check-in
            if (dto.CheckOutDate.Date <= dto.CheckInDate.Date)
                throw new Exception("Check-out date must be after the check-in date.");

            var room = await _roomRepo.GetByIdAsync(dto.RoomId);
            if (room == null || !room.IsAvailable)
                throw new Exception($"Room {dto.RoomId} is not available for booking.");

            var booking = new Booking
            {
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                NumberOfGuests = dto.NumberOfGuests,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status,
                GuestId = dto.GuestId,
                RoomId = dto.RoomId
            };

            var created = await _genericRepo.AddAsync(booking);

            // Mark room as unavailable
            room.IsAvailable = false;
            await _roomRepo.UpdateAsync(room);

            var bookingWithNav = await _genericRepo.GetAllQueryable()
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookingId == created.BookingId);

            return MapToReadDto(bookingWithNav!);
        }


        // Update booking
        public async Task<BookingReadDto?> UpdateAsync(int id, BookingUpdateDto dto)
        {
            var existing = await _genericRepo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.CheckInDate = dto.CheckInDate;
            existing.CheckOutDate = dto.CheckOutDate;
            existing.NumberOfGuests = dto.NumberOfGuests;
            existing.TotalAmount = dto.TotalAmount;
            existing.Status = dto.Status;
            existing.GuestId = dto.GuestId;
            existing.RoomId = dto.RoomId;

            var updated = await _genericRepo.UpdateAsync(existing);

            var bookingWithNav = await _genericRepo.GetAllQueryable()
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookingId == updated.BookingId);

            return MapToReadDto(bookingWithNav!);
        }

        // Delete booking → free up room
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _genericRepo.GetByIdAsync(id);
            if (existing == null) return false;

            var success = await _genericRepo.DeleteAsync(id);
            if (success)
            {
                var room = await _roomRepo.GetByIdAsync(existing.RoomId);
                if (room != null)
                {
                    room.IsAvailable = true;
                    await _roomRepo.UpdateAsync(room);
                }
            }
            return success;
        }

        // Maps Booking entity to BookingReadDto
        private static BookingReadDto MapToReadDto(Booking b)
        {
            return new BookingReadDto
            {
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                NumberOfGuests = b.NumberOfGuests,
                TotalAmount = b.TotalAmount,
                Status = b.Status,
                CreatedAt = b.CreatedAt,
                GuestName = b.Guest?.FullName ?? string.Empty,
                RoomNumber = b.Room?.RoomNumber ?? string.Empty
            };
        }
    }
}
