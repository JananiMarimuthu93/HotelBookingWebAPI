using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Services
{
    public class RoomService
    {
        private readonly IGenericRepository<Room> _genericRepo;
        private readonly IRoomRepository _roomRepo;

        public RoomService(IGenericRepository<Room> genericRepo, IRoomRepository roomRepo)
        {
            _genericRepo = genericRepo;
            _roomRepo = roomRepo;
        }

        // Get all rooms
        public async Task<IEnumerable<RoomReadDto>> GetAllAsync()
        {
            var rooms = await _genericRepo.GetAllQueryable()
                .Include(r => r.RoomType)
                .AsNoTracking()
                .ToListAsync();

            return rooms.Select(MapToReadDto);
        }

        // Get room by ID
        public async Task<RoomReadDto?> GetByIdAsync(int id)
        {
            var room = await _genericRepo.GetAllQueryable()
                .Include(r => r.RoomType)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null) return null;

            return MapToReadDto(room);
        }

        // Get all available rooms
        public async Task<IEnumerable<RoomReadDto>> GetAvailableRoomsAsync()
        {
            var rooms = await _roomRepo.GetAvailableRoomsAsync();
            return rooms.Select(MapToReadDto);
        }

        // Get rooms by RoomTypeId
        public async Task<IEnumerable<RoomReadDto>> GetRoomsByTypeAsync(int roomTypeId)
        {
            var rooms = await _roomRepo.GetRoomsByTypeAsync(roomTypeId);
            return rooms.Select(MapToReadDto);
        }

        // Create new room
        public async Task<RoomReadDto> CreateAsync(RoomCreateDto dto)
        {
            if (await _roomRepo.IsRoomNumberExistsAsync(dto.RoomNumber))
                throw new System.Exception("Room number already exists.");

            var room = new Room
            {
                RoomNumber = dto.RoomNumber,
                Capacity = dto.Capacity,
                PricePerDay = dto.PricePerDay,
                IsAvailable = dto.IsAvailable,
                Description = dto.Description,
                Floor = dto.Floor,
                ViewType = dto.ViewType,
                RoomTypeId = dto.RoomTypeId
            };

            var created = await _genericRepo.AddAsync(room);

            // Fetch with RoomType included
            var roomWithType = await _genericRepo.GetAllQueryable()
                .Include(r => r.RoomType)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoomId == created.RoomId);

            return MapToReadDto(roomWithType!);
        }

        // Update room
        public async Task<RoomReadDto?> UpdateAsync(int id, RoomUpdateDto dto)
        {
            var existing = await _genericRepo.GetByIdAsync(id);
            if (existing == null) return null;

            if (await _roomRepo.IsRoomNumberExistsAsync(dto.RoomNumber, id))
                throw new System.Exception("Room number already exists.");

            existing.RoomNumber = dto.RoomNumber;
            existing.Capacity = dto.Capacity;
            existing.PricePerDay = dto.PricePerDay;
            existing.IsAvailable = dto.IsAvailable;
            existing.Description = dto.Description;
            existing.Floor = dto.Floor;
            existing.ViewType = dto.ViewType;
            existing.RoomTypeId = dto.RoomTypeId;

            var updated = await _genericRepo.UpdateAsync(existing);

            // Fetch with RoomType included
            var roomWithType = await _genericRepo.GetAllQueryable()
                .Include(r => r.RoomType)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoomId == updated.RoomId);

            return MapToReadDto(roomWithType!);
        }

        // Delete room
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _genericRepo.GetByIdAsync(id);
            if (existing == null) return false;
            return await _genericRepo.DeleteAsync(id);
        }

        //maps Room entity to RoomReadDto
        private static RoomReadDto MapToReadDto(Room r)
        {
            return new RoomReadDto
            {
                RoomNumber = r.RoomNumber,
                Capacity = r.Capacity,
                PricePerDay = r.PricePerDay,
                IsAvailable = r.IsAvailable,
                Description = r.Description,
                Floor = r.Floor,
                ViewType = r.ViewType,
                CreatedAt = r.CreatedAt,
                RoomTypeName = r.RoomType?.TypeName ?? string.Empty
            };
        }
    }
}
