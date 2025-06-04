using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces
{
    public interface IUserPlantService
    {
        /// <summary>
        /// Gets a UserPlant by its identifier.
        /// </summary>
        /// <param name="userPlantId">The identifier of the UserPlant.</param>
        /// <returns>The UserPlant object, or null if not found.</returns>
        Task<Result<UserPlant>> GetUserPlant(Guid userPlantId);

        /// <summary>
        /// Gets all UserPlants for a specific user.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>A list of UserPlant objects.</returns>
        Task<Result<List<UserPlant>>> GetUserPlantsByUserId(Guid userId);

        /// <summary>
        /// Adds a new UserPlant entry.
        /// </summary>
        /// <param name="userPlant">The UserPlant to add.</param>
        /// <returns>The created UserPlant object.</returns>
        Task<Result<UserPlant>> AddUserPlant(UserPlant userPlant);

        /// <summary>
        /// Updates a UserPlant entry.
        /// </summary>
        /// <param name="userPlant">The UserPlant with updated data.</param>
        /// <returns>True if update was successful, false otherwise.</returns>
        Task<Result<bool>> UpdateUserPlant(UserPlant userPlant);

        /// <summary>
        /// Deletes a UserPlant by its identifier.
        /// </summary>
        /// <param name="userPlantId">The identifier of the UserPlant to delete.</param>
        /// <returns>True if deletion was successful, false otherwise.</returns>
        Task<Result<bool>> DeleteUserPlant(Guid userPlantId);
    }
}
