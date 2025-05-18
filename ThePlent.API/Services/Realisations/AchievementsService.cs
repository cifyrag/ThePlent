using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

/// <summary>
/// Implementation of the IAchievementsService interface for managing achievements.
/// </summary>
public class AchievementsService : IAchievementsService
{
    private readonly ILogger<AchievementsService> _logger;
    private readonly IGenericRepository<Achievement> _achievementRepository;
    private readonly IGenericRepository<UserAchievement> _userAchievementRepository; 

    /// <summary>
    /// Initializes a new instance of the AchievementsService class.
    /// </summary>
    /// <param name="logger">The logger for the service.</param>
    /// <param name="achievementRepository">The generic repository for Achievements.</param>
    /// <param name="userAchievementRepository">The generic repository for UserAchievements.</param>
    public AchievementsService(
        ILogger<AchievementsService> logger,
        IGenericRepository<Achievement> achievementRepository,
        IGenericRepository<UserAchievement> userAchievementRepository)
    {
        _logger = logger;
        _achievementRepository = achievementRepository;
        _userAchievementRepository = userAchievementRepository;
    }

    /// <summary>
    /// Adds a new achievement.
    /// </summary>
    /// <param name="achievementData">The data for the new achievement.</param>
    /// <returns>The newly added Achievement object, or an error.</returns>
    public async Task<Result<Achievement>> AddAchievement(Achievement achievementData)
    {
        try
        {
            if (achievementData == null)
            {
                return Error.Validation("Achievement data cannot be null.");
            }

            var result = await _achievementRepository.AddAsync(achievementData);

            if (!result.IsError)
            {
                return result.Value;
            }

            return result.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding an achievement.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Updates an existing achievement.
    /// </summary>
    /// <param name="achievementData">The updated achievement data.</param>
    /// <returns>True if the update was successful, or an error.</returns>
    public async Task<Result<bool>> UpdateAchievement(Achievement achievementData)
    {
        try
        {
             if (achievementData == null )
            {
                return Error.Validation("Valid achievement data with an ID is required for update.");
            }

            var result = await _achievementRepository.UpdateAsync(achievementData);

            if (!result.IsError)
            {
                return true;
            }

            return result.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating an achievement.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Removes an achievement by its identifier.
    /// </summary>
    /// <param name="achievementId">The identifier of the achievement to remove.</param>
    /// <returns>True if the removal was successful, or an error.</returns>
    public async Task<Result<bool>> RemoveAchievement(Guid achievementId)
    {
        try
        {
            var achievementToRemoveResult = await _achievementRepository.GetSingleAsync<Achievement>(
                a => a.AchievementId == achievementId);

            if (achievementToRemoveResult.IsError)
            {
                 return achievementToRemoveResult.Error;
            }

            if (achievementToRemoveResult?.Value == null)
            {
                return Error.NotFound($"Achievement with ID {achievementId} not found.");
            }

            var removeResult = await _achievementRepository.RemoveAsync(achievementToRemoveResult.Value);

            if (!removeResult.IsError)
            {
                return true;
            }

            return removeResult.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while removing an achievement.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Gets a single achievement by its identifier.
    /// </summary>
    /// <param name="achievementId">The identifier of the achievement.</param>
    /// <returns>The Achievement object, or a 404 Not Found error.</returns>
    public async Task<Result<Achievement>> GetAchievement(Guid achievementId)
    {
        try
        {
            var result = await _achievementRepository.GetSingleAsync<Achievement>(a => a.AchievementId == achievementId);

            if (!result.IsError)
            {
                 if (result?.Value == null)
                 {
                     return Error.NotFound($"Achievement with ID {achievementId} not found.");
                 }
                return result.Value;
            }

            return result.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting an achievement.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Gets a collection of all achievements, or achievements for a specific user.
    /// </summary>
    /// <param name="userId">Optional user identifier to get achievements for a specific user.</param>
    /// <returns>An enumerable collection of Achievement objects, or an error.</returns>
    public async Task<Result<System.Collections.Generic.IEnumerable<Achievement>>> GetAchievements(Guid? userId = null)
    {
        try
        {
            if (userId.HasValue )
            {
                var userAchievementsResult = await _userAchievementRepository.GetListAsync(
                    filter: ua => ua.UserId == userId.Value,
                    selector: ua => ua.AchievementId
                );

                if (userAchievementsResult.IsError)
                {
                    return Error.Unexpected("Failed to retrieve user achievement IDs.");
                }

                var userAchievementIds = userAchievementsResult.Value.ToList();

                if (!userAchievementIds.Any())
                {
                    return new List<Achievement>();
                }

                var achievementsResult = await _achievementRepository.GetListAsync<Achievement>(
                    filter: a => userAchievementIds.Contains(a.AchievementId)
                );

                if (!achievementsResult.IsError)
                {
                    return achievementsResult;
                }

                 return achievementsResult.Error;
            }
            else
            {
                var allAchievementsResult = await _achievementRepository.GetListAsync<Achievement>();

                if (!allAchievementsResult.IsError)
                {
                    return allAchievementsResult;
                }

                return allAchievementsResult.Error;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting achievements.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Marks an achievement as done for a user.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="achievementId">The identifier of the achievement.</param>
    /// <returns>True if the achievement was marked as done successfully, or an error.</returns>
    public async Task<Result<bool>> MarkAsDone(Guid userId, Guid achievementId)
    {
        try
        {
            var updateResult = await _userAchievementRepository.UpdateRangeAsync(
                filter: ua => ua.UserId == userId && ua.AchievementId == achievementId,
                updateExpression: calls => calls.SetProperty(ua => ua.Completed, true).SetProperty(ua =>ua.CompletedAt, DateTime.Now)
            );

            if (!updateResult.IsError)
            {
                 if (updateResult.Value == 0)
                 {
                      return Error.NotFound($"User Achievement link not found for User ID {userId} and Achievement ID {achievementId}.");
                 }

                 return true;
            }

            return updateResult.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while marking an achievement as done.");
            return Error.Unexpected();
        }
    }
    /// <summary>
    /// Shares an achievement.
    /// </summary>
    /// <param name="userId">The identifier of the user sharing the achievement.</param>
    /// <param name="achievementId">The identifier of the achievement to share.</param>
    /// <param name="platform">The platform to share on (e.g., "Twitter", "Facebook").</param>
    /// <returns>A Success object indicating the action was successful, or an error.</returns>
    public async Task<Result<Success>> ShareAchievement(Guid userId, Guid achievementId, string platform)
    {
        try
        {
            throw new NotImplementedException();
            _logger.LogInformation($"User {userId} shared achievement {achievementId} on platform {platform}.");

            return Success.Instance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sharing an achievement.");
            return Error.Unexpected();
        }
    }
}
