using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Services
{
    public class BookingService
    {
        private readonly IGenericRepository<Booking> _bookingRepoGeneric;
        private readonly IGenericRepository<Room> _roomRepo;
        private readonly IBookingRepository _bookingSpecialRepo;

        public BookingService(
            IGenericRepository<Booking> bookingRepoGeneric,
            IGenericRepository<Room> roomRepo,
            IBookingRepository bookingSpecialRepo)
        {
            _bookingRepoGeneric = bookingRepoGeneric;
            _roomRepo = roomRepo;
            _bookingSpecialRepo = bookingSpecialRepo;
        }

        
        public async Task<IEnumerable<BookingReadDto>> GetAllAsync()
        {
            var bookings = await _bookingRepoGeneric.GetAllQueryable()
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .AsNoTracking()
                .ToListAsync();

            return bookings.Select(MapToReadDto);
        }

        public async Task<BookingReadDto?> GetByIdAsync(int id)
        {
            var booking = await _bookingRepoGeneric.GetAllQueryable()
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookingId == id);

            return booking == null ? null : MapToReadDto(booking);
        }

        public async Task<int> GetTotalBookingsAsync()
        {
            return await _bookingSpecialRepo.GetTotalBookingsAsync();
        }

       
        public async Task<BookingReadDto> CreateAsync(BookingCreateDto dto)
        {
            ValidateDateRange(dto.CheckInDate, dto.CheckOutDate);

            var room = await _roomRepo.GetByIdAsync(dto.RoomId)
                       ?? throw new InvalidOperationException($"Room {dto.RoomId} does not exist.");

            if (dto.NumberOfGuests > room.Capacity)
                throw new InvalidOperationException($"Number of guests exceeds room capacity ({room.Capacity}).");

            var available = await _bookingSpecialRepo.IsRoomAvailableAsync(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);
            if (!available)
                throw new InvalidOperationException("The room is already booked during the selected dates.");

            var booking = new Booking
            {
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                NumberOfGuests = dto.NumberOfGuests,
                TotalAmount = dto.TotalAmount,
                Status = BookingStatus.Confirmed, 
                GuestId = dto.GuestId,
                RoomId = dto.RoomId
            };

            var created = await _bookingRepoGeneric.AddAsync(booking);

            var withNav = await _bookingRepoGeneric.GetAllQueryable()
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookingId == created.BookingId);

            return MapToReadDto(withNav!);
        }

        public async Task<BookingReadDto?> UpdateAsync(int id, BookingUpdateDto dto)
        {
            ValidateDateRange(dto.CheckInDate, dto.CheckOutDate);

            var existing = await _bookingRepoGeneric.GetByIdAsync(id);
            if (existing == null) return null;

            // Capacity check (room may be changed)
            var room = await _roomRepo.GetByIdAsync(dto.RoomId)
                       ?? throw new InvalidOperationException($"Room {dto.RoomId} does not exist.");

            if (dto.NumberOfGuests > room.Capacity)
                throw new InvalidOperationException($"Number of guests exceeds room capacity ({room.Capacity}).");

            // Overlap check excluding this booking
            var available = await _bookingSpecialRepo.IsRoomAvailableAsync(dto.RoomId, dto.CheckInDate, dto.CheckOutDate, id);
            if (!available)
                throw new InvalidOperationException("The room is already booked during the selected dates.");

            existing.CheckInDate = dto.CheckInDate;
            existing.CheckOutDate = dto.CheckOutDate;
            existing.NumberOfGuests = dto.NumberOfGuests;
            existing.TotalAmount = dto.TotalAmount;
            existing.GuestId = dto.GuestId;
            existing.RoomId = dto.RoomId;

           
            existing.Status = dto.Status;

            var updated = await _bookingRepoGeneric.UpdateAsync(existing);

            var withNav = await _bookingRepoGeneric.GetAllQueryable()
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookingId == updated.BookingId);

            return MapToReadDto(withNav!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _bookingRepoGeneric.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Status = BookingStatus.Cancelled;
            await _bookingRepoGeneric.UpdateAsync(existing);
            return true;
        }


        private static void ValidateDateRange(DateTime checkIn, DateTime checkOut)
        {
            var today = DateTime.UtcNow.Date;

            if (checkIn.Date < today)
                throw new InvalidOperationException("Check-in date cannot be in the past.");

            if (checkOut <= checkIn)
                throw new InvalidOperationException("Check-out date must be after the check-in date.");
        }

        private static BookingReadDto MapToReadDto(Booking b) => new()
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
