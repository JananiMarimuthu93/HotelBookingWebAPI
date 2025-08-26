using HotelBookingAPI.DTOs;
using HotelBookingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace HotelBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class RoomTypeController : ControllerBase
    {
        private readonly RoomTypeService _service;

        public RoomTypeController(RoomTypeService service)
        {
            _service = service;
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<RoomTypeReadDto>>> GetActive()
        {
            var result = await _service.GetAllActiveAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomTypeReadDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomTypeReadDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<RoomTypeReadDto>> Create(RoomTypeCreateDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoomTypeReadDto>> Update(int id, RoomTypeUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

       

    }
}
