using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;

namespace ThePlant.API.Controllers
{
    /// <summary>
    /// API controller for managing reminders.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] 
    public class RemindersController : ControllerBase
    {
        private readonly IRemindersService _remindersService;

        /// <summary>
        /// Constructor for the RemindersController.
        /// </summary>
        /// <param name="remindersService">The injected IRemindersService.</param>
        public RemindersController(IRemindersService remindersService)
        {
            _remindersService = remindersService;
        }

        /// <summary>
        /// Marks a specific reminder as done for a user.
        /// </summary>
        /// <param name="id">The identifier of the reminder (from route).</param>
        /// <param name="userId">The identifier of the user (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost("{id}/markdone")]
        public async Task<ActionResult> MarkAsDone(Guid id, [FromBody] Guid userId)
        {
            if (userId.Equals(Guid.Empty))
            {
                return BadRequest("Valid user ID is required.");
            }

            var result = await _remindersService.MarkAsDone(userId, id);

            
            if (!result.IsError)
            {
                return NoContent(); 
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Imports a reminder to a user's calendar.
        /// </summary>
        /// <param name="id">The identifier of the reminder (from route).</param>
        /// <param name="importRequest">Object containing user ID and optional calendar ID (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost("{id}/importtocalendar")]
        public async Task<ActionResult> ImportToCalendar(Guid id, [FromBody] ImportToCalendarRequest importRequest)
        {
            if (importRequest == null )
            {
                return BadRequest("Valid user ID is required for importing to calendar.");
            }

            var result = await _remindersService.ImportToCalendar(importRequest.UserId, id, importRequest.CalendarId);

            
            if (!result.IsError)
            {
                return Ok("Reminder imported to calendar successfully."); 
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets all reminders for a specific user.
        /// </summary>
        /// <param name="userId">The identifier of the user (from query string).</param>
        /// <returns>An ActionResult containing the collection of reminder objects for the user or an error.</returns>
        [HttpGet("user/{userId}")] // Route to get reminders for a specific user
        public async Task<ActionResult<IEnumerable<Reminder>>> GetUserReminders(Guid userId)
        {
            if (userId.Equals(Guid.Empty))
            {
                return BadRequest("Valid user ID is required.");
            }

            var result = await _remindersService.GetUserReminders(userId);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Creates a new reminder for a user.
        /// </summary>
        /// <param name="reminderData">The data for the new reminder.</param>
        /// <returns>An ActionResult containing the newly created reminder or an error.</returns>
        [HttpPost]
        public async Task<ActionResult<Reminder>> CreateReminder([FromBody] Reminder reminderData)
        {
            if (reminderData == null )
            {
                return BadRequest("Valid reminder data with a user ID is required.");
            }

            var result = await _remindersService.CreateReminder(reminderData);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Deletes a reminder by its identifier for a user.
        /// </summary>
        /// <param name="id">The identifier of the reminder to delete (from route).</param>
        /// <param name="userId">The identifier of the user (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReminder(Guid id, [FromBody] Guid userId)
        {
            if (userId.Equals(Guid.Empty))
            {
                return BadRequest("Valid user ID is required.");
            }

            var result = await _remindersService.DeleteReminder(userId, id);

            
            if (!result.IsError)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets upcoming reminders for a user within a specified timeframe.
        /// </summary>
        /// <param name="userId">The identifier of the user (from route).</param>
        /// <param name="timeframeInDays">The number of upcoming days to check (from query string, optional).</param>
        /// <returns>An ActionResult containing the collection of upcoming reminder objects for the user or an error.</returns>
        [HttpGet("user/{userId}/upcoming")] // Route to get upcoming reminders for a user
        public async Task<ActionResult<IEnumerable<Reminder>>> GetUpcomingReminders(Guid userId, [FromQuery] int timeframeInDays = 7)
        {
            if (userId.Equals(Guid.Empty))
            {
                return BadRequest("Valid user ID is required.");
            }

            var result = await _remindersService.GetUpcomingReminders(userId, timeframeInDays);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Updates an existing reminder.
        /// </summary>
        /// <param name="reminderData">The updated reminder data.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPut]
        public async Task<ActionResult> UpdateReminder([FromBody] Reminder reminderData)
        {
            if (reminderData == null )
            {
                return BadRequest("Valid reminder data with ID and User ID is required for update.");
            }

            var result = await _remindersService.UpdateReminder(reminderData);

            
            if (!result.IsError)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Represents the request body for importing a reminder to a calendar.
        /// </summary>
        public class ImportToCalendarRequest
        {
            /// <summary>
            /// The identifier of the user.
            /// </summary>
            public Guid UserId { get; set; }

            /// <summary>
            /// The identifier of the calendar to import to (optional).
            /// </summary>
            public string? CalendarId { get; set; }
        }
    }
}
