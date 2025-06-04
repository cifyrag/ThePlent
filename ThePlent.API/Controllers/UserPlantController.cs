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

        /// <summary>
        /// Gets all plants for a specific user.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>List of UserPlant</returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserPlant>>> GetUserPlants(Guid userId)
        {
            var result = await _userPlantService.GetUserPlants(userId);

            if (!result.IsError)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }
    }
}
