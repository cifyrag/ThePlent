using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Controllers
{
    /// <summary>
    /// API controller for managing feedback entries.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] 
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedBacksService _feedbacksService;

        /// <summary>
        /// Constructor for the FeedbacksController.
        /// </summary>
        /// <param name="feedbacksService">The injected IFeedBacksService.</param>
        public FeedbacksController(IFeedBacksService feedbacksService)
        {
            _feedbacksService = feedbacksService;
        }

        /// <summary>
        /// Adds a new feedback entry.
        /// </summary>
        /// <param name="feedbackData">The feedback object to add.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost]
        public async Task<ActionResult<Success>> AddFeedBack([FromBody] Feedback feedbackData)
        {
            if (feedbackData == null)
            {
                return BadRequest("Feedback data is required.");
            }

            var result = await _feedbacksService.AddFeedBack(feedbackData);

            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets a single feedback entry by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the feedback.</param>
        /// <returns>An ActionResult containing the feedback object or a 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedBack(Guid id)
        {
            var result = await _feedbacksService.GetFeedBack(id);

            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets a collection of feedback entries.
        /// </summary>
        /// <returns>An ActionResult containing the collection of feedback objects or an error.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedBacks()
        {
            var result = await _feedbacksService.GetFeedBacks();

            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }
    }
}
