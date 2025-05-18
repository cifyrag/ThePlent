using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces;

public interface ISubscriptionsService
{
    /// <summary>
    /// Checks the expiration date of a user's subscription.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>The expiration date of the subscription, or null if no active subscription is found.</returns>
    Task<Result<DateTime?>> CheckExpirationDate(int userId);

    /// <summary>
    /// Adds a new subscription for a user.
    /// </summary>
    /// <param name="subscriptionData">The data for the new subscription.</param>
    /// <returns>The newly added Subscription object, or null if adding fails.</returns>
    Task<Result<Subscription>> AddSubscription(Subscription subscriptionData);

    /// <summary>
    /// Initiates the subscription process for a user and a specific plan.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="planId">The identifier of the subscription plan.</param>
    /// <returns>True if the subscription was successful, false otherwise.</returns>
    Task<Result<bool>> Subscribe(int userId, int planId);

    /// <summary>
    /// Cancels an existing subscription.
    /// </summary>
    /// <param name="subscriptionId">The identifier of the subscription to cancel.</param>
    /// <returns>True if the cancellation was successful, false otherwise.</returns>
    Task<Result< bool>> CancelSubscription(int subscriptionId);
}

