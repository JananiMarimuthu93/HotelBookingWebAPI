using HotelBookingAPI.Models.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        // Get all available rooms
        Task<IEnumerable<Room>> GetAvailableRoomsAsync();

        // Get all rooms by RoomTypeId
        Task<IEnumerable<Room>> GetRoomsByTypeAsync(int roomTypeId);

        // Check if RoomNumber already exists (for create/update)
        Task<bool> IsRoomNumberExistsAsync(string roomNumber, int? excludeId = null);

        Task<IEnumerable<Room>> GetByCapacityAndFloorAsync(int capacity, string floor);
    }
}
