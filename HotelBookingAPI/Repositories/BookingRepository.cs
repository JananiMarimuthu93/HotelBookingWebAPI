using HotelBookingAPI.Context;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelBookingContext _context;

        public BookingRepository(HotelBookingContext context)
        {
            _context = context;
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null)
        {
            // Normalize to date precision if your data is date-only; else keep as is.
            // Overlap condition: (new.start < existing.end) && (new.end > existing.start)
            var hasOverlap = await _context.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                b.Status != BookingStatus.Cancelled &&
                b.Status != BookingStatus.CheckedOut &&
                (!excludeBookingId.HasValue || b.BookingId != excludeBookingId.Value) &&
                (checkIn < b.CheckOutDate && checkOut > b.CheckInDate));

            return !hasOverlap;
        }

        public async Task<int> GetTotalBookingsAsync()
        {
            return await _context.Bookings.CountAsync();
        }
    }
}
