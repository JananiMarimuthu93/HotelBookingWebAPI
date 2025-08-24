using HotelBookingAPI.Context;
using HotelBookingAPI.Models.AuthModels;
using HotelBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HotelBookingAPI.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly HotelBookingContext _context;

        public RoleRepository(HotelBookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync() => await _context.Roles.ToListAsync();

        public async Task<Role?> GetByIdAsync(string id) =>
            await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);

        public async Task<Role> AddAsync(Role role)
        {
            var lastRole = await _context.Roles
                .OrderByDescending(r => r.RoleId)
                .FirstOrDefaultAsync();

            role.RoleId = lastRole == null
                ? "R001"
                : "R" + (int.Parse(lastRole.RoleId.Substring(1)) + 1).ToString("D3");

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role?> UpdateAsync(string id, Role role)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null) return null;

            existing.RoleName = role.RoleName;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null) return false;

            _context.Roles.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
