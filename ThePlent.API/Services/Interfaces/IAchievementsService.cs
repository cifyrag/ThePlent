using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces;

public interface IAchievementsService
{
    /// <summary>
    /// Adds a new achievement.
    /// </summary>
    /// <param name="achievementData">The data for the new achievement.</param>
    /// <returns>The newly added Achievement object, or null if adding fails.</returns>
    Task<Result<Achievement>> AddAchievement(Achievement achievementData); 

        /// <summary>
        /// Updates an existing achievement.
        /// </summary>
        /// <param name="achievementData">The updated achievement data.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<Result<bool>> UpdateAchievement(Achievement achievementData); 

        /// <summary>
        /// Removes an achievement by its identifier.
        /// </summary>
        /// <param name="achievementId">The identifier of the achievement to remove.</param>
        /// <returns>True if the removal was successful, false otherwise.</returns>
        Task<Result<bool>> RemoveAchievement(int achievementId); 

        /// <summary>
        /// Gets a single achievement by its identifier.
        /// </summary>
        /// <param name="achievementId">The identifier of the achievement.</param>
        /// <returns>The Achievement object, or null if not found.</returns>
        Task<Result<Achievement>> GetAchievement(int achievementId); 

        /// <summary>
        /// Gets a collection of all achievements, or achievements for a specific user.
        /// </summary>
        /// <param name="userId">Optional user identifier to get achievements for a specific user.</param>
        /// <returns>An enumerable collection of Achievement objects.</returns>
        Task<Result<System.Collections.Generic.IEnumerable<Achievement>>> GetAchievements(int? userId = null);

        /// <summary>
        /// Marks an achievement as done for a user.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="achievementId">The identifier of the achievement.</param>
        /// <returns>True if the achievement was marked as done successfully, false otherwise.</returns>
        Task<Result<bool>> MarkAsDone(int userId, int achievementId); 

        /// <summary>
        /// Shares an achievement.
        /// </summary>
        /// <param name="userId">The identifier of the user sharing the achievement.</param>
        /// <param name="achievementId">The identifier of the achievement to share.</param>
        /// <param name="platform">The platform to share on (e.g., "Twitter", "Facebook").</param>
        Task<Result<Success>> ShareAchievement(int userId, int achievementId, string platform); 
}