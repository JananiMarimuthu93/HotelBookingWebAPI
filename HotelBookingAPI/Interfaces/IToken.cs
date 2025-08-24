using HotelBookingAPI.Models.AuthModels;

namespace HotelBookingAPI.Interfaces
{
        public interface IToken
        {
            string GenerateToken(User user);
        }
}
