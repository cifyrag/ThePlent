
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

/// <summary>
/// Implementation of the ISubscriptionsService interface for managing user subscriptions.
/// </summary>
public class SubscriptionsService : ISubscriptionsService
{
    private readonly ILogger<SubscriptionsService> _logger;
    private readonly IGenericRepository<Subscription> _subscriptionRepository;
    private readonly IGenericRepository<UserSubscription> _userSubscriptionRepository; 

    /// <summary>
    /// Initializes a new instance of the SubscriptionsService class.
    /// </summary>
    /// <param name="logger">The logger for the service.</param>
    /// <param name="subscriptionRepository">The generic repository for Subscription plans.</param>
    /// <param name="userSubscriptionRepository">The generic repository for UserSubscription links.</param>
    public SubscriptionsService(
        ILogger<SubscriptionsService> logger,
        IGenericRepository<Subscription> subscriptionRepository,
        IGenericRepository<UserSubscription> userSubscriptionRepository)
    {
        _logger = logger;
        _subscriptionRepository = subscriptionRepository;
        _userSubscriptionRepository = userSubscriptionRepository;
    }

    /// <summary>
    /// Checks the expiration date of a user's active subscription.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>The expiration date of the subscription, or null if no active subscription is found, or an error.</returns>
    public async Task<Result<DateTime?>> CheckExpirationDate(Guid userId)
    {
         try
         {
             var activeSubscriptionResult = await _userSubscriptionRepository.GetSingleAsync<UserSubscription>(
                 filter: us => us.UserId == userId && us.EndDate > DateTime.UtcNow
             );

             if (activeSubscriptionResult.IsError)
             {
                 return activeSubscriptionResult.Error;
             }

             return activeSubscriptionResult.Value?.EndDate;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while checking subscription expiration date.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Adds a new subscription plan.
    /// </summary>
    /// <param name="subscriptionData">The data for the new subscription plan.</param>
    /// <returns>The newly added Subscription object, or an error.</returns>
    public async Task<Result<Subscription>> AddSubscription(Subscription subscriptionData)
    {
         try
         {
             if (subscriptionData == null)
             {
                 return Error.Validation("Subscription data cannot be null.");
             }

             var result = await _subscriptionRepository.AddAsync(subscriptionData);

             if (!result.IsError)
             {
                 return result.Value;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while adding a subscription plan.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Initiates the subscription process for a user and a specific plan.
    /// This typically involves creating a UserSubscription entry.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="planId">The identifier of the subscription plan.</param>
    /// <returns>True if the subscription was successful, or an error.</returns>
    public async Task<Result<bool>> Subscribe(Guid userId, Guid planId)
    {
         try
         {
             var planResult = await _subscriptionRepository.ExistsAsync(s => s.SubscriptionId == planId); 
             
             if (planResult.IsError)
             {
                 return planResult.Error;
             }

             if (!planResult.Value)
             {
                 return Error.NotFound($"Subscription plan with ID {planId} not found.");
             }

             var newUserSubscription = new UserSubscription
             {
                 UserId = userId,
                 SubscriptionId = planId,
                 StartDate = DateTime.UtcNow,
                 EndDate = DateTime.UtcNow.AddYears(1),
             };

             var addResult = await _userSubscriptionRepository.AddAsync(newUserSubscription);

             if (!addResult.IsError)
             {
                 return true;
             }

             return addResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while subscribing a user to a plan.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Cancels an existing user subscription.
    /// This typically involves updating the status or end date of the UserSubscription entry.
    /// </summary>
    /// <param name="subscriptionId">The identifier of the user's subscription entry (UserSubscriptionId).</param>
    /// <returns>True if the cancellation was successful, or an error.</returns>
    public async Task<Result<bool>> CancelSubscription(Guid subscriptionId)
    {
         try
         {
             var updateResult = await _userSubscriptionRepository.UpdateRangeAsync(
                 filter: us => us.UserSubscriptionId == subscriptionId, 
                 updateExpression: calls => calls
                     .SetProperty(us => us.EndDate, DateTime.UtcNow) 
             );

             if (!updateResult.IsError)
             {
                 if (updateResult.Value == 0)
                 {
                     return Error.NotFound($"User Subscription with ID {subscriptionId} not found.");
                 }
                 return true;
             }

             return updateResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while cancelling a subscription.");
             return Error.Unexpected();
         }
    }
}
