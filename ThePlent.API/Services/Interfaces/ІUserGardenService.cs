using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces;

public interface ІUserGardenService
{
/// <summary>
        /// Adds a plant to a user's garden.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="plantId">The identifier of the plant to add.</param>
        /// <returns>The newly added UserPlant object, or null if adding fails.</returns>
        Task<Result<UserPlant>> AddUserPlant(Guid userId, Guid plantId); 

        /// <summary>
        /// Deletes a plant from a user's garden.
        /// </summary>
        /// <param name="userPlantId">The identifier of the user's plant entry.</param>
        /// <returns>True if the deletion was successful, false otherwise.</returns>
        Task<Result<bool>> DeleteUserPlant(Guid userPlantId); 
        
        /// <summary>
        /// Renames a plant in a user's garden.
        /// </summary>
        /// <param name="userPlantId">The identifier of the user's plant entry.</param>
        /// <param name="newName">The new custom name for the plant.</param>
        /// <returns>True if the rename was successful, false otherwise.</returns>
        Task<Result<bool>> RenameUserPlant(Guid userPlantId, string newName);
        
        /// <summary>
        /// Updates the details of a plant in a user's garden.
        /// </summary>
        /// <param name="userPlantData">The updated user plant data.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<Result< bool>> UpdateUserPlant(UserPlant userPlantData); 
        
        /// <summary>
        /// Updates only the custom name of a plant in a user's garden.
        /// </summary>
        /// <param name="userPlantId">The identifier of the user's plant entry.</param>
        /// <param name="newName">The new custom name for the plant.</param>
        /// <returns>True if the name update was successful, false otherwise.</returns>
        Task<Result<bool>> UpdateUserPlantName(Guid userPlantId, string newName); 
        
}