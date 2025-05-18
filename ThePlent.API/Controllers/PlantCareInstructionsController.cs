using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;

namespace ThePlant.API.Controllers
{
    /// <summary>
    /// API controller for managing plant care instructions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] 
    public class PlantCareInstructionsController : ControllerBase
    {
        private readonly IPlantCareInstructionsService _plantCareInstructionsService;

        /// <summary>
        /// Constructor for the PlantCareInstructionsController.
        /// </summary>
        /// <param name="plantCareInstructionsService">The injected IPlantCareInstructionsService.</param>
        public PlantCareInstructionsController(IPlantCareInstructionsService plantCareInstructionsService)
        {
            _plantCareInstructionsService = plantCareInstructionsService;
        }

        /// <summary>
        /// Adds a new plant care instruction.
        /// </summary>
        /// <param name="careInstructionData">The data for the new care instruction.</param>
        /// <returns>An ActionResult containing the newly added instruction or an error.</returns>
        [HttpPost]
        public async Task<ActionResult<PlantCareInstruction>> AddCareInstruction([FromBody] PlantCareInstruction careInstructionData)
        {
            if (careInstructionData == null)
            {
                return BadRequest("Plant care instruction data is required.");
            }

            var result = await _plantCareInstructionsService.AddCareInstruction(careInstructionData);

            if (!result.IsError)
            {
                return CreatedAtAction(nameof(GetCareInstruction), new { id = result.Value.PlantCareInstructionId }, result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets a single plant care instruction by its identifier or associated plant identifier.
        /// </summary>
        /// <param name="id">The identifier of the care instruction or the plant.</param>
        /// <returns>An ActionResult containing the instruction object or a 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlantCareInstruction>> GetCareInstruction(Guid id)
        {
            var result = await _plantCareInstructionsService.GetCareInstruction(id);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Updates an existing plant care instruction.
        /// </summary>
        /// <param name="careInstructionData">The updated care instruction data.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPut]
        public async Task<ActionResult> UpdateCareInstruction([FromBody] PlantCareInstruction careInstructionData)
        {
            if (careInstructionData == null )
            {
                return BadRequest("Valid plant care instruction data with an ID is required for update.");
            }

            var result = await _plantCareInstructionsService.UpdateCareInstruction(careInstructionData);

            
            if (!result.IsError)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Removes a plant care instruction by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the care instruction to remove.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCareInstruction(Guid id)
        {
            var result = await _plantCareInstructionsService.RemoveCareInstruction(id);

            
            if (!result.IsError)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }
    }
}
