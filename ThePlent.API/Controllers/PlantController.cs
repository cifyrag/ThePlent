using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;

namespace ThePlant.API.Controllers
{
    /// <summary>
    /// API controller for managing plant entries.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")] 
    public class PlantController : ControllerBase
    {
        private readonly IPlantService _plantService;

        /// <summary>
        /// Constructor for the PlantController.
        /// </summary>
        /// <param name="plantService">The injected IPlantService.</param>
        public PlantController(IPlantService plantService)
        {
            _plantService = plantService;
        }

        /// <summary>
        /// Adds a new plant.
        /// </summary>
        /// <param name="plantData">The data for the new plant.</param>
        /// <returns>An ActionResult containing the newly added plant or an error.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Plant>> AddPlant([FromBody] Plant plantData)
        {
            if (plantData == null)
            {
                return BadRequest("Plant data is required.");
            }

            var result = await _plantService.AddPlant(plantData);

            
            if (!result.IsError)
            {
                return CreatedAtAction(nameof(GetPlant), new { id = result.Value.PlantId }, result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Updates an existing plant.
        /// </summary>
        /// <param name="plantData">The updated plant data.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdatePlant([FromBody] Plant plantData)
        {
            if (plantData == null )
            {
                return BadRequest("Valid plant data with an ID is required for update.");
            }

            var result = await _plantService.UpdatePlant(plantData);

            
            if (!result.IsError)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets a single plant by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the plant.</param>
        /// <returns>An ActionResult containing the plant or a 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(Guid id)
        {
            var result = await _plantService.GetPlant(id);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets a collection of plants.
        /// </summary>
        /// <returns>An ActionResult containing the collection of plants or an error.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
        {
            var result = await _plantService.GetPlants();

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Removes a plant by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the plant to remove.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemovePlant(Guid id)
        {
            var result = await _plantService.RemovePlant(id);

            
            if (!result.IsError)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Retrieves a collection of plants, possibly for display purposes.
        /// </summary>
        /// <returns>An ActionResult containing the collection of plants or an error.</returns>
        [HttpGet("show")] // Using a distinct route for ShowPlants
        public async Task<ActionResult<IEnumerable<Plant>>> ShowPlants()
        {
            var result = await _plantService.ShowPlants();

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets the care instructions for a specific plant.
        /// </summary>
        /// <param name="plantId">The identifier of the plant.</param>
        /// <returns>An ActionResult containing the care instructions or a 404 Not Found.</returns>
        [HttpGet("{plantId}/careinstructions")] 
        public async Task<ActionResult<PlantCareInstruction>> ViewPlantCareInstructions(Guid plantId)
        {
            var result = await _plantService.ViewPlantCareInstructions(plantId);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }
    }
}
