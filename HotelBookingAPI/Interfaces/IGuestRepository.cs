namespace HotelBookingAPI.Interfaces
{
    public interface IAuthRepository
    {
        // Check if Email or Phone already exists (for create or update)
        Task<bool> IsEmailExistsAsync(string email, int? excludeId = null);
        Task<bool> IsPhoneExistsAsync(string phone, int? excludeId = null);
    }
}
