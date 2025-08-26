using HotelBookingAPI.DTOs;
using HotelBookingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/Booking
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<BookingReadDto>>> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }

        // GET: api/Booking/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Manager,Guests")]
        public async Task<ActionResult<BookingReadDto>> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        // POST: api/Booking
        [HttpPost]
        [Authorize(Roles = "Manager,Guests")]
        [HttpPost]
        [Authorize(Roles = "Manager,Guests")]
        public async Task<ActionResult<BookingReadDto>> Create([FromBody] BookingCreateDto dto)
        {
            try
            {
                var created = await _bookingService.CreateAsync(dto);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // PUT: api/Booking/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<BookingReadDto>> Update(int id, [FromBody] BookingUpdateDto dto)
        {
            var updated = await _bookingService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/Booking/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _bookingService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
