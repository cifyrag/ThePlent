using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;

namespace ThePlant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlantImagesController : ControllerBase
    {
        private readonly IPlantImagesService _plantImagesService;

        public PlantImagesController(IPlantImagesService plantImagesService)
        {
            _plantImagesService = plantImagesService;
        }

        [HttpPost]

        public async Task<ActionResult<PlantImage>> AddImage([FromBody] PlantImage imageData)
        {
            if (imageData == null)
                return BadRequest("Image data is required.");

            var result = await _plantImagesService.AddImage(imageData);

            if (!result.IsError)
            {
                return CreatedAtAction(nameof(GetImage), new { id = result.Value.PlantImageId }, result.Value);
            }

            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlantImage>> GetImage(Guid id)
        {
            var result = await _plantImagesService.GetImage(id);

            return !result.IsError ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("plant/{plantId}")] // New GET endpoint
        public async Task<ActionResult<IEnumerable<PlantImage>>> GetImagesByPlantId(Guid plantId)
        {
            var result = await _plantImagesService.GetImagesByPlantId(plantId);

            return !result.IsError ? Ok(result.Value) : BadRequest(result.Error);
        }


        [HttpPut]

        public async Task<ActionResult> UpdateImage([FromBody] PlantImage imageData)
        {
            if (imageData == null)
                return BadRequest("Valid image data is required for update.");

            var result = await _plantImagesService.UpdateImage(imageData);

            return !result.IsError ? NoContent() : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> RemoveImage(Guid id)
        {
            var result = await _plantImagesService.RemoveImage(id);

            return !result.IsError ? NoContent() : BadRequest(result.Error);
        }
    }
}