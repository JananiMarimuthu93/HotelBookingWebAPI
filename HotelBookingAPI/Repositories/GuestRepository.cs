using HotelBookingAPI.Context;
using HotelBookingAPI.Interfaces;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repositories.Implementations
{
    public class GuestRepository : IAuthRepository
    {
        private readonly HotelBookingContext _context;

        public GuestRepository(HotelBookingContext context)
        {
            _context = context;
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeId = null)
        {
            return await _context.Guests
                .AnyAsync(g => g.Email.ToLower() == email.ToLower() &&
                               (!excludeId.HasValue || g.GuestId != excludeId.Value));
        }

        public async Task<bool> IsPhoneExistsAsync(string phone, int? excludeId = null)
        {
            return await _context.Guests
                .AnyAsync(g => g.Phone == phone &&
                               (!excludeId.HasValue || g.GuestId != excludeId.Value));
        }
    }
}
