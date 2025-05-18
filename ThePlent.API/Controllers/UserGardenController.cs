using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;

namespace ThePlant.API.Controllers
{
    /// <summary>
    /// API controller for managing a user's garden.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] 
    public class UserGardenController : ControllerBase
    {
        private readonly ІUserGardenService _userGardenService;

        /// <summary>
        /// Constructor for the UserGardenController.
        /// </summary>
        /// <param name="userGardenService">The injected IUserGardenService.</param>
        public UserGardenController(ІUserGardenService userGardenService)
        {
            _userGardenService = userGardenService;
        }

        /// <summary>
        /// Adds a plant to a user's garden.
        /// </summary>
        /// <param name="addPlantRequest">Object containing user ID and plant ID (from body).</param>
        /// <returns>An ActionResult containing the newly added UserPlant object or an error.</returns>
        [HttpPost] // POST api/UserGarden
        public async Task<ActionResult<UserPlant>> AddUserPlant([FromBody] AddUserPlantRequest addPlantRequest)
        {
            if (addPlantRequest == null )
            {
                return BadRequest("Valid user ID and plant ID are required to add a plant to the garden.");
            }

            var result = await _userGardenService.AddUserPlant(addPlantRequest.UserId, addPlantRequest.PlantId);

            
            if (!result.IsError)
            {
                 return CreatedAtAction(nameof(GetUserPlant), new { id = result.Value.PlantId }, result.Value);
            }
            return BadRequest(result.Error);
        }

         /// <summary>
        /// Gets a single user plant entry by its identifier.
        /// This is a helper action for CreatedAtAction in AddUserPlant.
        /// You might want a dedicated GetUserPlant(int id) method in your service.
        /// </summary>
        /// <param name="id">The identifier of the user plant entry.</param>
        /// <returns>An ActionResult containing the user plant or a 404 Not Found.</returns>
        [HttpGet("{id}")] // GET api/UserGarden/{id}
        public async Task<ActionResult<UserPlant>> GetUserPlant(int id)
        {
             // This is a placeholder. You would need a corresponding method in your IUserGardenService
             // to actually fetch a UserPlant by its ID if you want to use CreatedAtAction effectively.
             // Consider adding a Task<Result<UserPlant>> GetUserPlant(int id); to your interface.
             return NotFound("GetUserPlant by ID is not fully implemented in this example controller.");
        }


        /// <summary>
        /// Deletes a plant from a user's garden.
        /// </summary>
        /// <param name="id">The identifier of the user's plant entry to delete (from route).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("{id}")] // DELETE api/UserGarden/{id}
        public async Task<ActionResult> DeleteUserPlant(Guid id)
        {
            var result = await _userGardenService.DeleteUserPlant(id);

            
            if (!result.IsError)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Renames a plant in a user's garden.
        /// </summary>
        /// <param name="id">The identifier of the user's plant entry (from route).</param>
        /// <param name="renameRequest">Object containing the new custom name (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost("{id}/rename")] // POST api/UserGarden/{id}/rename
        public async Task<ActionResult> RenameUserPlant(Guid id, [FromBody] RenameUserPlantRequest renameRequest)
        {
            if (renameRequest == null || string.IsNullOrEmpty(renameRequest.NewName))
            {
                return BadRequest("New name is required for renaming.");
            }

            var result = await _userGardenService.RenameUserPlant(id, renameRequest.NewName);

            
            if (!result.IsError)
            {
                return NoContent(); 
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Updates the details of a plant in a user's garden.
        /// </summary>
        /// <param name="userPlantData">The updated user plant data (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPut] // PUT api/UserGarden
        public async Task<ActionResult> UpdateUserPlant([FromBody] UserPlant userPlantData)
        {
            if (userPlantData == null )
            {
                return BadRequest("Valid user plant data with an ID is required for update.");
            }

            var result = await _userGardenService.UpdateUserPlant(userPlantData);

            
            if (!result.IsError)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Updates only the custom name of a plant in a user's garden.
        /// </summary>
        /// <param name="id">The identifier of the user's plant entry (from route).</param>
        /// <param name="updateNameRequest">Object containing the new custom name (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPut("{id}/name")] // PUT api/UserGarden/{id}/name
        public async Task<ActionResult> UpdateUserPlantName(Guid id, [FromBody] UpdateUserPlantNameRequest updateNameRequest)
        {
            if (updateNameRequest == null || string.IsNullOrEmpty(updateNameRequest.NewName))
            {
                return BadRequest("New name is required for updating plant name.");
            }

            var result = await _userGardenService.UpdateUserPlantName(id, updateNameRequest.NewName);

            
            if (!result.IsError)
            {
                return NoContent(); 
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Represents the request body for the AddUserPlant action.
        /// </summary>
        public class AddUserPlantRequest
        {
            /// <summary>
            /// The identifier of the user.
            /// </summary>
            public Guid UserId { get; set; }

            /// <summary>
            /// The identifier of the plant to add.
            /// </summary>
            public Guid PlantId { get; set; }
        }

        /// <summary>
        /// Represents the request body for the RenameUserPlant action.
        /// </summary>
        public class RenameUserPlantRequest
        {
            /// <summary>
            /// The new custom name for the plant.
            /// </summary>
            public string NewName { get; set; }
        }

        /// <summary>
        /// Represents the request body for the UpdateUserPlantName action.
        /// </summary>
        public class UpdateUserPlantNameRequest
        {
            /// <summary>
            /// The new custom name for the plant.
            /// </summary>
            public string NewName { get; set; }
        }
    }
}
