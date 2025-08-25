using HotelBookingAPI.Context;
using HotelBookingAPI.Interfaces;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repositories.Implementations
{
    public class GuestRepository : IGuestRepository
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
        public async Task<Guest?> GetByEmailAsync(string email)
        {
            return await _context.Guests
                .Include(g => g.Bookings)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Email.ToLower() == email.ToLower());
        }

        public async Task<Guest?> GetByPhoneAsync(string phone)
        {
            return await _context.Guests
                .Include(g => g.Bookings)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Phone == phone);
        }

        public async Task<IEnumerable<Guest>> SearchByNameAsync(string name)
        {
            return await _context.Guests
                .Where(g => g.FullName.ToLower().Contains(name.ToLower()))
                .Include(g => g.Bookings)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Guest>> GetGuestsCreatedAfterAsync(DateTime date)
        {
            return await _context.Guests
                .Where(g => g.CreatedAt >= date)
                .Include(g => g.Bookings)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Guest>> GetTopGuestsByBookingsAsync(int count)
        {
            return await _context.Guests
                .Include(g => g.Bookings)
                .OrderByDescending(g => g.Bookings.Count)
                .Take(count)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
