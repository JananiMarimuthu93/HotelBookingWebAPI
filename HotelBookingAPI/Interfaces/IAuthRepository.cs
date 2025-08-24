using HotelBookingAPI.Models.AuthModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HotelBookingAPI.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(string id);
        Task<Role> AddAsync(Role role);
        Task<Role?> UpdateAsync(string id, Role role);
        Task<bool> DeleteAsync(string id);
    }

    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task<User> AddAsync(User user);
        Task<User?> UpdateAsync(string id, User user);
        Task<bool> DeleteAsync(string id);
        Task<bool> IsEmailExistsAsync(string email, string? excludeId = null);
       
        Task<User?> GetLastUserAsync();
        Task<User?> GetByEmailAsync(string email);

    }
}
