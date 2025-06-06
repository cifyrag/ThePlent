using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;

namespace ThePlant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPlantController : ControllerBase
    {
        private readonly IUserPlantService _userPlantService;

        public UserPlantController(IUserPlantService userPlantService)
        {
            _userPlantService = userPlantService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserPlant>>> GetUserPlants(Guid userId)
        {
            var result = await _userPlantService.GetUserPlants(userId);

            if (!result.IsError)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<ActionResult<UserPlant>> AddUserPlant([FromBody] UserPlant userPlant)
        {
            if (userPlant == null)
                return BadRequest("UserPlant data is required.");

            var result = await _userPlantService.AddUserPlant(userPlant);

            if (!result.IsError)
                return CreatedAtAction(nameof(GetUserPlants), new { userId = result.Value.UserId }, result.Value);

            return BadRequest(result.Error);
        }

        [HttpPut("{userPlantId}")] // Added Update endpoint
        public async Task<ActionResult<UserPlant>> UpdateUserPlant(Guid userPlantId, [FromBody] UserPlant userPlant)
        {
            if (userPlant == null)
                return BadRequest("UserPlant data is required.");

            if (userPlantId != userPlant.UserPlantId)
                return BadRequest("UserPlant ID in URL does not match ID in body.");

            var result = await _userPlantService.UpdateUserPlant(userPlant);

            if (!result.IsError)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }

        [HttpDelete("{userPlantId}")]
        public async Task<ActionResult> DeleteUserPlant(Guid userPlantId)
        {
            var result = await _userPlantService.DeleteUserPlant(userPlantId);

            if (!result.IsError)
                return NoContent();

            return BadRequest(result.Error);
        }
    }
}