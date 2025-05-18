using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;

namespace ThePlant.API.Controllers
{
    /// <summary>
    /// API controller for managing achievements.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementsController : ControllerBase
    {
        private readonly IAchievementsService _achievementsService;

        /// <summary>
        /// Constructor for the AchievementsController.
        /// </summary>
        /// <param name="achievementsService">The injected IAchievementsService.</param>
        public AchievementsController(IAchievementsService achievementsService)
        {
            _achievementsService = achievementsService;
        }

        /// <summary>
        /// Adds a new achievement.
        /// </summary>
        /// <param name="achievementData">The data for the new achievement.</param>
        /// <returns>An ActionResult containing the newly added achievement or an error.</returns>
        [HttpPost]
        public async Task<ActionResult<Achievement>> AddAchievement([FromBody] Achievement achievementData)
        {
            if (achievementData == null)
            {
                return BadRequest("Achievement data is required.");
            }

            var result = await _achievementsService.AddAchievement(achievementData);

             if (!result.IsError)
            {
                return CreatedAtAction(nameof(GetAchievement), new { id = result.Value.AchievementId }, result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Updates an existing achievement.
        /// </summary>
        /// <param name="achievementData">The updated achievement data.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPut]
        public async Task<ActionResult> UpdateAchievement([FromBody] Achievement achievementData)
        {
            if (achievementData == null )
            {
                return BadRequest("Valid achievement data with an ID is required for update.");
            }

            var result = await _achievementsService.UpdateAchievement(achievementData);

            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Removes an achievement by its identifier.
        /// </summary>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveAchievement(Guid id)
        {
            var result = await _achievementsService.RemoveAchievement(id);

            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets a single achievement by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the achievement.</param>
        /// <returns>An ActionResult containing the achievement or a 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Achievement>> GetAchievement(Guid id)
        {
            var result = await _achievementsService.GetAchievement(id);

             if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets a collection of all achievements, or achievements for a specific user.
        /// </summary>
        /// <param name="userId">Optional user identifier to get achievements for a specific user (from query string).</param>
        /// <returns>An ActionResult containing the collection of achievements or an error.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Achievement>>> GetAchievements([FromQuery] Guid? userId = null)
        {
            var result = await _achievementsService.GetAchievements(userId);

             if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Marks an achievement as done for a user.
        /// </summary>
        /// <param name="id">The identifier of the achievement (from route).</param>
        /// <param name="userId">The identifier of the user (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost("{id}/markdone")]
        public async Task<ActionResult> MarkAsDone(Guid id, [FromBody] Guid userId)
        {
            var result = await _achievementsService.MarkAsDone(userId, id);

            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Shares an achievement.
        /// </summary>
        /// <param name="id">The identifier of the achievement (from route).</param>
        /// <param name="shareRequest">Object containing user ID and platform (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost("{id}/share")]
        public async Task<ActionResult> ShareAchievement(Guid id, [FromBody] ShareAchievementRequest shareRequest)
        {
            if (shareRequest == null || string.IsNullOrEmpty(shareRequest.Platform))
            {
                return BadRequest("Valid user ID and platform are required for sharing.");
            }

            var result = await _achievementsService.ShareAchievement(shareRequest.UserId, id, shareRequest.Platform);

             if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Represents the request body for sharing an achievement.
        /// </summary>
        public class ShareAchievementRequest
        {
            /// <summary>
            /// The identifier of the user sharing the achievement.
            /// </summary>
            public Guid UserId { get; set; }

            /// <summary>
            /// The platform to share on (e.g., "Twitter", "Facebook").
            /// </summary>
            public string Platform { get; set; }
        }
    }
}
