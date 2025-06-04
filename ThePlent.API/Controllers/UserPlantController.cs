using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;

namespace ThePlant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserPlantController : ControllerBase
    {
        private readonly IUserPlantService _userPlantService;

        public UserPlantController(IUserPlantService userPlantService)
        {
            _userPlantService = userPlantService;
        }

        // GET api/UserPlant/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPlant>> GetUserPlant(Guid id)
        {
            var result = await _userPlantService.GetUserPlant(id);

            if (result.IsError)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        // GET api/UserPlant/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<UserPlant>>> GetUserPlantsByUserId(Guid userId)
        {
            var result = await _userPlantService.GetUserPlantsByUserId(userId);

            if (result.IsError)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        // POST api/UserPlant
        [HttpPost]
        public async Task<ActionResult<UserPlant>> AddUserPlant([FromBody] UserPlant userPlant)
        {
            if (userPlant == null)
                return BadRequest("UserPlant data is required.");

            var result = await _userPlantService.AddUserPlant(userPlant);

            if (result.IsError)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetUserPlant), new { id = result.Value.UserPlantId }, result.Value);
        }

        // DELETE api/UserPlant/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserPlant(Guid id)
        {
            var result = await _userPlantService.DeleteUserPlant(id);

            if (result.IsError)
                return BadRequest(result.Error);

            return NoContent();
        }

        
        // PUT api/UserPlant
        [HttpPut]
        public async Task<ActionResult> UpdateUserPlant([FromBody] UserPlant userPlant)
        {
            if (userPlant == null)
                return BadRequest("UserPlant data is required.");

            var result = await _userPlantService.UpdateUserPlant(userPlant);

            if (result.IsError)
                return BadRequest(result.Error);

            return NoContent();
        }

        

        public class RenameUserPlantRequest
        {
            public string NewName { get; set; }
        }

        public class UpdateUserPlantNameRequest
        {
            public string NewName { get; set; }
        }
    }
}
