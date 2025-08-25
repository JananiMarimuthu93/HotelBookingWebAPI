using HotelBookingAPI.DTOs;
using HotelBookingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GuestController : ControllerBase
    {
        private readonly GuestService _guestService;

        public GuestController(GuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<GuestReadDto>>> GetAll()
        {
            var guests = await _guestService.GetAllAsync();
            return Ok(guests);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<GuestReadDto>> GetById(int id)
        {
            var guest = await _guestService.GetByIdAsync(id);
            if (guest == null) return NotFound();
            return Ok(guest);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<GuestReadDto>> Create([FromBody] GuestCreateDto dto)
        {
            var created = await _guestService.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<GuestReadDto>> Update(int id, [FromBody] GuestUpdateDto dto)
        {
            var updated = await _guestService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _guestService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("by-email/{email}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var guest = await _guestService.GetByEmailAsync(email);
            if (guest == null) return NotFound($"Guest with email '{email}' not found.");
            return Ok(guest);
        }

        [HttpGet("by-phone/{phone}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            var guest = await _guestService.GetByPhoneAsync(phone);
            if (guest == null) return NotFound($"Guest with phone '{phone}' not found.");
            return Ok(guest);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            var guests = await _guestService.SearchByNameAsync(name);
            if (!guests.Any()) return NotFound($"No guests found with name containing '{name}'.");
            return Ok(guests);
        }

        [HttpGet("top-guests")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetTopGuestsByBookings([FromQuery] int count = 5)
        {
            var guests = await _guestService.GetTopGuestsByBookingsAsync(count);
            if (!guests.Any()) return NotFound("No guests found.");
            return Ok(guests);
        }
    }
}
