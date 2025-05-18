
using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;

namespace ThePlant.API.Controllers
{
    /// <summary>
    /// API controller for managing subscriptions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] 
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionsService _subscriptionsService;

        /// <summary>
        /// Constructor for the SubscriptionsController.
        /// </summary>
        /// <param name="subscriptionsService">The injected ISubscriptionsService.</param>
        public SubscriptionsController(ISubscriptionsService subscriptionsService)
        {
            _subscriptionsService = subscriptionsService;
        }

        /// <summary>
        /// Checks the expiration date of a user's subscription.
        /// </summary>
        /// <param name="userId">The identifier of the user (from route).</param>
        /// <returns>An ActionResult containing the expiration date or a 404 Not Found.</returns>
        [HttpGet("user/{userId}/expiration")] // GET api/Subscriptions/user/{userId}/expiration
        public async Task<ActionResult<DateTime?>> CheckExpirationDate(Guid userId)
        {
            if (userId.Equals(Guid.Empty))
            {
                return BadRequest("Valid user ID is required.");
            }

            var result = await _subscriptionsService.CheckExpirationDate(userId);

            
            if (!result.IsError)
            {
                if (result.Value.HasValue)
                {
                    return Ok(result.Value.Value);
                }
                else
                {
                    return NotFound("No active subscription found for this user.");
                }
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Adds a new subscription.
        /// </summary>
        /// <param name="subscriptionData">The data for the new subscription.</param>
        /// <returns>An ActionResult containing the newly added subscription or an error.</returns>
        [HttpPost] // POST api/Subscriptions
        public async Task<ActionResult<Subscription>> AddSubscription([FromBody] Subscription subscriptionData)
        {
            if (subscriptionData == null )
            {
                return BadRequest("Valid subscription data with a user ID is required.");
            }

            var result = await _subscriptionsService.AddSubscription(subscriptionData);

            
            if (!result.IsError)
            {
                // Return 201 Created with the newly created subscription
                // Assuming you have a GetSubscriptionById method to use with CreatedAtAction
                // If not, you might just return Ok(result.Value);
                 return CreatedAtAction(nameof(GetSubscription), new { id = result.Value.SubscriptionId }, result.Value);
            }
            return BadRequest(result.Error);
        }

         /// <summary>
        /// Gets a single subscription by its identifier.
        /// This is a helper action for CreatedAtAction in AddSubscription.
        /// You might want a dedicated GetSubscription(int id) method in your service.
        /// </summary>
        /// <param name="id">The identifier of the subscription.</param>
        /// <returns>An ActionResult containing the subscription or a 404 Not Found.</returns>
        [HttpGet("{id}")] // GET api/Subscriptions/{id}
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
             // This is a placeholder. You would need a corresponding method in your ISubscriptionsService
             // to actually fetch a subscription by its ID if you want to use CreatedAtAction effectively.
             // For now, it just returns a placeholder response.
             // Consider adding a Task<Result<Subscription>> GetSubscription(int id); to your interface.
             return NotFound("GetSubscription by ID is not fully implemented in this example controller.");
        }


        /// <summary>
        /// Initiates the subscription process for a user and a specific plan.
        /// </summary>
        /// <param name="subscribeRequest">Object containing user ID and plan ID (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost("subscribe")] // POST api/Subscriptions/subscribe
        public async Task<ActionResult> Subscribe([FromBody] SubscribeRequest subscribeRequest)
        {
            if (subscribeRequest == null )
            {
                return BadRequest("Valid user ID and plan ID are required for subscribing.");
            }

            var result = await _subscriptionsService.Subscribe(subscribeRequest.UserId, subscribeRequest.PlanId);

            
            if (!result.IsError)
            {
                return Ok("Subscription successful."); 
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Cancels an existing subscription.
        /// </summary>
        /// <param name="id">The identifier of the subscription to cancel (from route).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost("{id}/cancel")] // POST api/Subscriptions/{id}/cancel
        public async Task<ActionResult> CancelSubscription(Guid id)
        {
            var result = await _subscriptionsService.CancelSubscription(id);

            
            if (!result.IsError)
            {
                return NoContent(); 
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Represents the request body for the Subscribe action.
        /// </summary>
        public class SubscribeRequest
        {
            /// <summary>
            /// The identifier of the user.
            /// </summary>
            public Guid UserId { get; set; }

            /// <summary>
            /// The identifier of the subscription plan.
            /// </summary>
            public Guid PlanId { get; set; }
        }

         /// <summary>
        /// Represents the request body for the CancelSubscription action.
        /// </summary>
        public class CancelSubscriptionRequest
        {
             /// <summary>
            /// The identifier of the subscription to cancel.
            /// </summary>
            public Guid SubscriptionId { get; set; }
        }
    }
}
