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
            return !await _context.Bookings
                .AnyAsync(b =>
                    b.RoomId == roomId &&
                    (!excludeBookingId.HasValue || b.BookingId != excludeBookingId.Value) &&
                    ((checkIn >= b.CheckInDate && checkIn < b.CheckOutDate) ||
                     (checkOut > b.CheckInDate && checkOut <= b.CheckOutDate) ||
                     (checkIn <= b.CheckInDate && checkOut >= b.CheckOutDate))
                );
        }
    }
}
