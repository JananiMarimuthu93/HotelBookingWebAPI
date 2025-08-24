using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models.AuthModels;
using HotelBookingAPI.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepo;

        public RoleService(IRoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task<IEnumerable<RoleReadDto>> GetAllAsync()
        {
            var roles = await _roleRepo.GetAllAsync();
            return roles.Select(r => new RoleReadDto { RoleId = r.RoleId, RoleName = r.RoleName });
        }

        public async Task<RoleReadDto?> GetByIdAsync(string id)
        {
            var r = await _roleRepo.GetByIdAsync(id);
            if (r == null) return null;
            return new RoleReadDto { RoleId = r.RoleId, RoleName = r.RoleName };
        }

        public async Task<RoleReadDto> CreateAsync(RoleCreateDto dto)
        {
            var role = new Role { RoleName = dto.RoleName };
            var created = await _roleRepo.AddAsync(role);
            return new RoleReadDto { RoleId = created.RoleId, RoleName = created.RoleName };
        }

        public async Task<RoleReadDto?> UpdateAsync(string id, RoleUpdateDto dto)
        {
            var updated = await _roleRepo.UpdateAsync(id, new Role { RoleName = dto.RoleName });
            if (updated == null) return null;
            return new RoleReadDto { RoleId = updated.RoleId, RoleName = updated.RoleName };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _roleRepo.DeleteAsync(id);
        }
    }
}
