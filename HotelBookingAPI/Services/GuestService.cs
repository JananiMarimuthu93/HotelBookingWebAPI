using HotelBookingAPI.DTOs;
using HotelBookingAPI.Interfaces;
using HotelBookingAPI.Models.DomainModels;
using HotelBookingAPI.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingAPI.Services
{
    public class GuestService
    {
        private readonly IGenericRepository<Guest> _genericRepo;
        private readonly IAuthRepository _guestRepo;

        public GuestService(IGenericRepository<Guest> genericRepo, IAuthRepository guestRepo)
        {
            _genericRepo = genericRepo;
            _guestRepo = guestRepo;
        }

        public async Task<IEnumerable<GuestReadDto>> GetAllAsync()
        {
            var guests = await _genericRepo.GetAllAsync();
            return guests.Select(g => new GuestReadDto
            {
                GuestId = g.GuestId,
                FullName = g.FullName,
                Email = g.Email,
                Phone = g.Phone,
                Address = g.Address,
                CreatedAt = g.CreatedAt,
                TotalBookings = g.Bookings?.Count ?? 0
            });
        }

        public async Task<GuestReadDto?> GetByIdAsync(int id)
        {
            var guest = await _genericRepo.GetByIdAsync(id);
            if (guest == null) return null;

            return new GuestReadDto
            {
                GuestId = guest.GuestId,
                FullName = guest.FullName,
                Email = guest.Email,
                Phone = guest.Phone,
                Address = guest.Address,
                CreatedAt = guest.CreatedAt,
                TotalBookings = guest.Bookings?.Count ?? 0
            };
        }

        public async Task<GuestReadDto> CreateAsync(GuestCreateDto dto)
        {
            if (await _guestRepo.IsEmailExistsAsync(dto.Email))
                throw new System.Exception("Email already exists.");

            if (await _guestRepo.IsPhoneExistsAsync(dto.Phone))
                throw new System.Exception("Phone already exists.");

            var guest = new Guest
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address
            };

            var created = await _genericRepo.AddAsync(guest);

            return new GuestReadDto
            {
                GuestId = created.GuestId,
                FullName = created.FullName,
                Email = created.Email,
                Phone = created.Phone,
                Address = created.Address,
                CreatedAt = created.CreatedAt,
                TotalBookings = 0
            };
        }

        public async Task<GuestReadDto?> UpdateAsync(int id, GuestUpdateDto dto)
        {
            var existing = await _genericRepo.GetByIdAsync(id);
            if (existing == null) return null;

            if (await _guestRepo.IsEmailExistsAsync(dto.Email, id))
                throw new System.Exception("Email already exists.");

            if (await _guestRepo.IsPhoneExistsAsync(dto.Phone, id))
                throw new System.Exception("Phone already exists.");

            existing.FullName = dto.FullName;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            existing.Address = dto.Address;

            var updated = await _genericRepo.UpdateAsync(existing);

            return new GuestReadDto
            {
                GuestId = updated.GuestId,
                FullName = updated.FullName,
                Email = updated.Email,
                Phone = updated.Phone,
                Address = updated.Address,
                CreatedAt = updated.CreatedAt,
                TotalBookings = updated.Bookings?.Count ?? 0
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
