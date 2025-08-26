using HotelBookingAPI.DTOs;
using HotelBookingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingAPI.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/Room
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomReadDto>>> GetAll()
        {
            var rooms = await _roomService.GetAllAsync();
            return Ok(rooms);
        }

        // GET: api/Room/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomReadDto>> GetById(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        // GET: api/Room/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<RoomReadDto>>> GetAvailableRooms()
        {
            var rooms = await _roomService.GetAvailableRoomsAsync();
            return Ok(rooms);
        }

        // GET: api/Room/by-type/{roomTypeId}
        [HttpGet("by-type/{roomTypeId}")]
        public async Task<ActionResult<IEnumerable<RoomReadDto>>> GetRoomsByType(int roomTypeId)
        {
            var rooms = await _roomService.GetRoomsByTypeAsync(roomTypeId);
            return Ok(rooms);
        }

        // POST: api/Room
        [HttpPost]
        public async Task<ActionResult<RoomReadDto>> Create([FromBody] RoomCreateDto dto)
        {
            var created = await _roomService.CreateAsync(dto);
            return Ok(created);
        }

        // PUT: api/Room/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<RoomReadDto>> Update(int id, [FromBody] RoomUpdateDto dto)
        {
            var updated = await _roomService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/Room/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _roomService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("Capacity/{capacity}/Floor/{floor}")]
        public async Task<ActionResult<IEnumerable<RoomTypeReadDto>>> GetRoomTypesByCapacityAndFloor(int capacity, string floor)
        {
            var filteredRoomTypes = await _roomService.GetByCapacityAndFloorAsync(capacity, floor);

            if (filteredRoomTypes == null || !filteredRoomTypes.Any())
                return NotFound($"No room types found with Capacity '{capacity}' on Floor '{floor}'.");

            return Ok(filteredRoomTypes);
        }
    }
}
