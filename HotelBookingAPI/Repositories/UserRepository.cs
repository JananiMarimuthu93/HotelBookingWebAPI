using HotelBookingAPI.Context;
using HotelBookingAPI.Models.AuthModels;
using HotelBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HotelBookingAPI.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly HotelBookingContext _context;

        public UserRepository(HotelBookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.Include(u => u.Role).ToListAsync();

        public async Task<User?> GetByIdAsync(string id) =>
            await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);

        public async Task<User> AddAsync(User user)
        {
            var lastUser = await _context.Users
                .OrderByDescending(u => u.UserId)
                .FirstOrDefaultAsync();

            user.UserId = lastUser == null
                ? "U001"
                : "U" + (int.Parse(lastUser.UserId.Substring(1)) + 1).ToString("D3");

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(string id, User user)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null) return null;

            existing.UserName = user.UserName;
            existing.Email = user.Email;
            existing.PasswordHash = user.PasswordHash;
            existing.RoleId = user.RoleId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null) return false;

            _context.Users.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsEmailExistsAsync(string email, string? excludeId = null)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.UserId != excludeId);
        }
        public async Task<User?> GetLastUserAsync()
        {
            return await _context.Users
                .OrderByDescending(u => u.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetByEmailAsync(string email) =>await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }
}
