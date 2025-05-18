using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

public class SubscriptionsService: ISubscriptionsService
{
    private readonly ILogger<SubscriptionsService> _logger;

    public SubscriptionsService(ILogger<SubscriptionsService> logger)
    {
        _logger = logger;
    }

    public async Task<Result<DateTime?>> CheckExpirationDate(int userId)
    {
         try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");

            return Error.Unexpected();
        }
    }

    public async Task<Result<Subscription>> AddSubscription(Subscription subscriptionData)
    {
         try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");

            return Error.Unexpected();
        }
    }

    public async Task<Result<bool>> Subscribe(int userId, int planId)
    {
         try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");

            return Error.Unexpected();
        }
    }

    public async Task<Result<bool>> CancelSubscription(int subscriptionId)
    {
         try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");

            return Error.Unexpected();
        }
    }
}