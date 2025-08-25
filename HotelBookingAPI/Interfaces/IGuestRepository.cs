using HotelBookingAPI.Models.DomainModels;

namespace HotelBookingAPI.Interfaces
{
    public interface IGuestRepository
    {
        // Check if Email or Phone already exists (for create or update)
        Task<bool> IsEmailExistsAsync(string email, int? excludeId = null);
        Task<bool> IsPhoneExistsAsync(string phone, int? excludeId = null);

        Task<Guest?> GetByEmailAsync(string email);
        Task<Guest?> GetByPhoneAsync(string phone);
        Task<IEnumerable<Guest>> SearchByNameAsync(string name);
        Task<IEnumerable<Guest>> GetGuestsCreatedAfterAsync(DateTime date);
        Task<IEnumerable<Guest>> GetTopGuestsByBookingsAsync(int count);
    }
}
