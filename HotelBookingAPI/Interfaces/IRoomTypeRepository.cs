using HotelBookingAPI.Models.DomainModels;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repositories.Interfaces
{
    public interface IRoomTypeRepository
    {
        Task<IEnumerable<RoomType>> GetActiveRoomTypesAsync();
        Task<bool> IsTypeNameExistsAsync(string typeName, int? excludeId = null);
    }
}
