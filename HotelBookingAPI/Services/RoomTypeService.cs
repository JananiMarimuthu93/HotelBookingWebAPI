using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Services
{
    public class RoomTypeService
    {
        private readonly IGenericRepository<RoomType> _genericRepo;
        private readonly IRoomTypeRepository _roomTypeRepo;

        public RoomTypeService(IGenericRepository<RoomType> genericRepo, IRoomTypeRepository roomTypeRepo)
        {
            _genericRepo = genericRepo;
            _roomTypeRepo = roomTypeRepo;
        }

        public async Task<IEnumerable<RoomTypeReadDto>> GetAllActiveAsync()
        {
            var roomTypes = await _roomTypeRepo.GetActiveRoomTypesAsync();
            return roomTypes.Select(rt => new RoomTypeReadDto
            {
                RoomTypeId = rt.RoomTypeId,
                TypeName = rt.TypeName,
                Description = rt.Description,
                IsActive = rt.IsActive,
                CreatedAt = rt.CreatedAt
            });
        }

        public async Task<RoomTypeReadDto?> GetByIdAsync(int id)
        {
            var rt = await _genericRepo.GetByIdAsync(id);
            if (rt == null) return null;

            return new RoomTypeReadDto
            {
                RoomTypeId = rt.RoomTypeId,
                TypeName = rt.TypeName,
                Description = rt.Description,
                IsActive = rt.IsActive,
                CreatedAt = rt.CreatedAt
            };
        }

        public async Task<RoomTypeReadDto> CreateAsync(RoomTypeCreateDTO dto)
        {
            if (await _roomTypeRepo.IsTypeNameExistsAsync(dto.TypeName))
                throw new System.Exception("Room Type Name already exists.");

            var roomType = new RoomType
            {
                TypeName = dto.TypeName,
                Description = dto.Description,
                IsActive = dto.IsActive
            };

            var created = await _genericRepo.AddAsync(roomType);

            return new RoomTypeReadDto
            {
                RoomTypeId = created.RoomTypeId,
                TypeName = created.TypeName,
                Description = created.Description,
                IsActive = created.IsActive,
                CreatedAt = created.CreatedAt
            };
        }

        public async Task<RoomTypeReadDto?> UpdateAsync(int id, RoomTypeUpdateDto dto)
        {
            var existing = await _genericRepo.GetByIdAsync(id);
            if (existing == null) return null;

            if (await _roomTypeRepo.IsTypeNameExistsAsync(dto.TypeName, id))
                throw new System.Exception("Room Type Name already exists.");

            existing.TypeName = dto.TypeName;
            existing.Description = dto.Description;
            existing.IsActive = dto.IsActive;

            var updated = await _genericRepo.UpdateAsync(existing);

            return new RoomTypeReadDto
            {
                RoomTypeId = updated.RoomTypeId,
                TypeName = updated.TypeName,
                Description = updated.Description,
                IsActive = updated.IsActive,
                CreatedAt = updated.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _genericRepo.GetByIdAsync(id);
            if (existing == null) return false;

            return await _genericRepo.DeleteAsync(id);
        }
    }
}
