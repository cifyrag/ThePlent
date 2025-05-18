using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

public class AchievementsService : IAchievementsService
{
    private readonly ILogger<AchievementsService> _logger;

    public AchievementsService(ILogger<AchievementsService> logger)
    {
        _logger = logger;
    }

    public async Task<Result<Achievement>> AddAchievement(Achievement achievementData)
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

    public async Task<Result<bool>> UpdateAchievement(Achievement achievementData)
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

    public async Task<Result<bool>> RemoveAchievement(int achievementId)
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

    public async Task<Result<Achievement>> GetAchievement(int achievementId)
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

    public async Task<Result<IEnumerable<Achievement>>> GetAchievements(int? userId = null)
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

    public async Task<Result<bool>> MarkAsDone(int userId, int achievementId)
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

    public async Task<Result<Success>> ShareAchievement(int userId, int achievementId, string platform)
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