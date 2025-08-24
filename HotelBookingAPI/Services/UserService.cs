using HotelBookingAPI.DTOs;
using HotelBookingAPI.Models.AuthModels;
using HotelBookingAPI.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace HotelBookingAPI.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return users.Select(u => new UserReadDto
            {
                UserId = u.UserId!,
                UserName = u.UserName!,
                Email = u.Email!,
                RoleId = u.RoleId!,
                RoleName = u.Role?.RoleName ?? string.Empty
            });
        }

        public async Task<UserReadDto?> GetByIdAsync(string id)
        {
            var u = await _userRepo.GetByIdAsync(id);
            if (u == null) return null;

            return new UserReadDto
            {
                UserId = u.UserId!,
                UserName = u.UserName!,
                Email = u.Email!,
                RoleId = u.RoleId!,
                RoleName = u.Role?.RoleName ?? string.Empty
            };
        }

       

    public async Task<UserReadDto> CreateAsync(UserCreateDto dto)
    {
        if (await _userRepo.IsEmailExistsAsync(dto.Email))
            throw new System.Exception("Email already exists.");

        var lastUser = await _userRepo.GetLastUserAsync();
        string newId = lastUser == null
            ? "U001"
            : "U" + (int.Parse(lastUser.UserId!.Substring(1)) + 1).ToString("D3");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            UserId = newId,
            UserName = dto.UserName,
            Email = dto.Email,
            PasswordHash = hashedPassword,
            RoleId = dto.RoleId
        };

        var created = await _userRepo.AddAsync(user);

        return new UserReadDto
        {
            UserId = created.UserId!,
            UserName = created.UserName!,
            Email = created.Email!,
            RoleId = created.RoleId!,
            RoleName = created.Role?.RoleName ?? string.Empty
        };
    }


        public async Task<UserReadDto?> UpdateAsync(string id, UserUpdateDto dto)
        {
            var existing = await _userRepo.GetByIdAsync(id);
            if (existing == null) return null;

      
            existing.UserName = dto.UserName;
            existing.Email = dto.Email;
            existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            existing.RoleId = dto.RoleId;

            // Pass both ID and updated User entity
            var updated = await _userRepo.UpdateAsync(id, existing);

            return new UserReadDto
            {
                UserId = updated.UserId!,
                UserName = updated.UserName!,
                Email = updated.Email!,
                RoleId = updated.RoleId!,
                RoleName = updated.Role?.RoleName ?? string.Empty
            };
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _userRepo.GetByIdAsync(id);
            if (existing == null) return false;
            return await _userRepo.DeleteAsync(id);
        }
        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null) return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash!);
            return isValid ? user : null;
        }
    }


}
