using HotelBookingAPI.Models.DomainModels;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        // Check if Room is available for given date range
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null);
    }
}
