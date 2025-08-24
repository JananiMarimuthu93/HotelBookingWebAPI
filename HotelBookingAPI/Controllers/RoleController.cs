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
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleReadDto>>> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        // GET: api/Role/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleReadDto>> GetById(string id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        // POST: api/Role
        [HttpPost]
        public async Task<ActionResult<RoleReadDto>> Create([FromBody] RoleCreateDto dto)
        {
            var createdRole = await _roleService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdRole.RoleId }, createdRole);
        }

        // PUT: api/Role/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<RoleReadDto>> Update(string id, [FromBody] RoleUpdateDto dto)
        {
            var updated = await _roleService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/Role/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var deleted = await _roleService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
