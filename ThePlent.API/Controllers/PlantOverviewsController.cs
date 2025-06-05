using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;

namespace ThePlant.API.Controllers
{
    /// <summary>
    /// API controller for managing plant overview entries.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] 
    [Authorize]
    public class PlantOverviewsController : ControllerBase
    {
        private readonly IPlantOverviewService _plantOverviewService;

        /// <summary>
        /// Constructor for the PlantOverviewsController.
        /// </summary>
        /// <param name="plantOverviewService">The injected IPlantOverviewService.</param>
        public PlantOverviewsController(IPlantOverviewService plantOverviewService)
        {
            _plantOverviewService = plantOverviewService;
        }

        /// <summary>
        /// Adds a new plant overview entry.
        /// </summary>
        /// <param name="plantOverviewData">The data for the new plant overview.</param>
        /// <returns>An ActionResult containing the newly added overview or an error.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PlantOverview>> AddPlant_Overview([FromBody] PlantOverview plantOverviewData)
        {
            if (plantOverviewData == null)
            {
                return BadRequest("Plant overview data is required.");
            }

            var result = await _plantOverviewService.AddPlant_Overview(plantOverviewData);

            
            if (!result.IsError)
            {
                return CreatedAtAction(nameof(GetPlant_Overview), new { id = result.Value.PlantOverviewId }, result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets a single plant overview entry by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the plant overview.</param>
        /// <returns>An ActionResult containing the overview object or a 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlantOverview>> GetPlant_Overview(Guid id)
        {
            var result = await _plantOverviewService.GetPlant_Overview(id);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Deletes a plant overview entry by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the plant overview to delete.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePlant_Overview(Guid id)
        {
            var result = await _plantOverviewService.DeletePlant_Overview(id);

            
            if (!result.IsError)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }
        /// <summary>
        /// Gets all plant overviews for a specific Plant ID.
        /// </summary>
        /// <param name="plantId">The identifier of the Plant (from route).</param>
        /// <returns>An ActionResult containing the collection of plant overview objects for the Plant or an error.</returns>
        [HttpGet("plant/{plantId}")]
        public async Task<ActionResult<IEnumerable<PlantOverview>>> GetPlantOverviewsByPlantId(Guid plantId)
        {
            if (plantId.Equals(Guid.Empty))
            {
                return BadRequest("Valid Plant ID is required.");
            }

            var result = await _plantOverviewService.GetPlantOverviewsByPlantId(plantId);

            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

    }
}
