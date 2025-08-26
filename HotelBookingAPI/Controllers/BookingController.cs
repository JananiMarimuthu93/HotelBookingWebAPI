using HotelBookingAPI.DTOs;
using HotelBookingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Manager,Guest")]
        public async Task<ActionResult<BookingReadDto>> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        // POST: api/Booking
        [HttpPost]
        [Authorize(Roles = "Manager,Guest")]
        public async Task<ActionResult<BookingReadDto>> Create([FromBody] BookingCreateDto dto)
        {
            try
            {
                var created = await _bookingService.CreateAsync(dto);
                return (created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/Booking/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<BookingReadDto>> Update(int id, [FromBody] BookingUpdateDto dto)
        {
            try
            {
                var updated = await _bookingService.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Booking/{id}  -> soft delete (Cancel)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _bookingService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        // GET: api/Booking/total
        [HttpGet("total")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetTotalBookings()
        {
            var total = await _bookingService.GetTotalBookingsAsync();
            return Ok(total);
        }
    }
}
