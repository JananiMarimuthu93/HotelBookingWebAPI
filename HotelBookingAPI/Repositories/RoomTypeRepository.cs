using HotelBookingAPI.Context;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repositories.Implementations
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly HotelBookingContext _context;

        public RoomTypeRepository(HotelBookingContext context)
        {
            _context = context;
        }
        // Check if TypeName already exists(for create or update)
        public async Task<bool> IsTypeNameExistsAsync(string typeName, int? excludeId = null)
        {
            return await _context.RoomTypes
                .AnyAsync(rt => rt.TypeName.ToLower() == typeName.ToLower() &&
                                (!excludeId.HasValue || rt.RoomTypeId != excludeId.Value));
        }
        // Get all active room types
        public async Task<IEnumerable<RoomType>> GetActiveRoomTypesAsync()
        {
            return await _context.RoomTypes
                .Where(rt => rt.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
