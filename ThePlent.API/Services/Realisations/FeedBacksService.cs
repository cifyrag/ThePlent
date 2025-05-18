using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

public class FeedBacksService: IFeedBacksService
{
    private readonly ILogger<FeedBacksService> _logger;

    public FeedBacksService(ILogger<FeedBacksService> logger)
    {
        _logger = logger;
    }

    public async Task<Result<Success>> AddFeedBack(Feedback feedback)
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

    public async Task<Result<Feedback>> GetFeedBack(int id)
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

    public async Task<Result<IEnumerable<Feedback>>> GetFeedBacks()
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