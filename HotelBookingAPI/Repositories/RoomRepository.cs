using HotelBookingAPI.Context;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repositories.Implementations
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelBookingContext _context;

        public RoomRepository(HotelBookingContext context)
        {
            _context = context;
        }
        // Get all available rooms
        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
        {
            return await _context.Rooms
                .Where(r => r.IsAvailable)
                .Include(r => r.RoomType)
                .AsNoTracking()
                .ToListAsync();
        }
        // Get all rooms by RoomTypeId
        public async Task<IEnumerable<Room>> GetRoomsByTypeAsync(int roomTypeId)
        {
            return await _context.Rooms
                .Where(r => r.RoomTypeId == roomTypeId)
                .Include(r => r.RoomType)
                .AsNoTracking()
                .ToListAsync();
        }
        // Check if RoomNumber already exists(for create or update)
        public async Task<bool> IsRoomNumberExistsAsync(string roomNumber, int? excludeId = null)
        {
            return await _context.Rooms
                .AnyAsync(r => r.RoomNumber.ToLower() == roomNumber.ToLower() &&
                               (!excludeId.HasValue || r.RoomId != excludeId.Value));
        }
    }
}
