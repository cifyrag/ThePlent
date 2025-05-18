using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

/// <summary>
/// Implementation of the IUserGardenService interface for managing a user's garden.
/// </summary>
public class UserGardenService : ІUserGardenService
{
    private readonly ILogger<UserGardenService> _logger;
    private readonly IGenericRepository<UserPlant> _userPlantRepository; 
    private readonly IGenericRepository<Plant> _plantRepository; 


    /// <summary>
    /// Initializes a new instance of the UserGardenService class.
    /// </summary>
    /// <param name="logger">The logger for the service.</param>
    /// <param name="userPlantRepository">The generic repository for UserPlant.</param>
    /// <param name="plantRepository">The generic repository for Plant.</param>
    public UserGardenService(
        ILogger<UserGardenService> logger,
        IGenericRepository<UserPlant> userPlantRepository,
        IGenericRepository<Plant> plantRepository)
    {
        _logger = logger;
        _userPlantRepository = userPlantRepository;
        _plantRepository = plantRepository;
    }

    /// <summary>
    /// Adds a plant to a user's garden.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="plantId">The identifier of the plant to add.</param>
    /// <returns>The newly added UserPlant object, or an error.</returns>
    public async Task<Result<UserPlant>> AddUserPlant(Guid userId, Guid plantId)
    {
         try
         {
             var plantExistsResult = await _plantRepository.ExistsAsync(p => p.PlantId == plantId);

             if (plantExistsResult.IsError)
             {
                 return plantExistsResult.Error;
             }

             if (!plantExistsResult.Value)
             {
                 return Error.NotFound($"Plant with ID {plantId} not found.");
             }

             var newUserPlant = new UserPlant
             {
                 UserId = userId,
                 PlantId = plantId,
             };

             var addResult = await _userPlantRepository.AddAsync(newUserPlant);

             if (!addResult.IsError)
             {
                 return addResult.Value;
             }

             return addResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while adding a plant to the user's garden.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Deletes a plant from a user's garden.
    /// </summary>
    /// <param name="userPlantId">The identifier of the user's plant entry.</param>
    /// <returns>True if the deletion was successful, or an error.</returns>
    public async Task<Result<bool>> DeleteUserPlant(Guid userPlantId)
    {
         try
         {
             var userPlantToRemoveResult = await _userPlantRepository.GetSingleAsync<UserPlant>(up => up.UserPlantId == userPlantId);

             if (userPlantToRemoveResult.IsError)
             {
                 return userPlantToRemoveResult.Error;
             }

             if (userPlantToRemoveResult?.Value == null)
             {
                 return Error.NotFound($"User plant entry with ID {userPlantId} not found.");
             }

             var removeResult = await _userPlantRepository.RemoveAsync(userPlantToRemoveResult.Value);

             if (!removeResult.IsError)
             {
                 return true;
             }

             return removeResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while deleting a plant from the user's garden.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Renames a plant in a user's garden.
    /// </summary>
    /// <param name="userPlantId">The identifier of the user's plant entry.</param>
    /// <param name="newName">The new custom name for the plant.</param>
    /// <returns>True if the rename was successful, or an error.</returns>
    public async Task<Result<bool>> RenameUserPlant(Guid userPlantId, string newName)
    {
         try
         {
             if (string.IsNullOrEmpty(newName))
             {
                 return Error.Validation("New name cannot be empty.");
             }

             var updateResult = await _userPlantRepository.UpdateRangeAsync(
                 filter: up => up.UserPlantId == userPlantId,
                 updateExpression: calls => calls.SetProperty(up => up.UserPlantName, newName) 
             );

             if (!updateResult.IsError)
             {
                  if (updateResult.Value == 0)
                  {
                       return Error.NotFound($"User plant entry with ID {userPlantId} not found.");
                  }
                  return true;
             }

             return updateResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while renaming a plant in the user's garden.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Updates the details of a plant in a user's garden.
    /// </summary>
    /// <param name="userPlantData">The updated user plant data.</param>
    /// <returns>True if the update was successful, or an error.</returns>
    public async Task<Result<bool>> UpdateUserPlant(UserPlant userPlantData)
    {
         try
         {
              if (userPlantData == null)
              {
                  return Error.Validation("Valid user plant data is required for update.");
              }

              var result = await _userPlantRepository.UpdateAsync(userPlantData);

              if (!result.IsError)
              {
                  return true;
              }

              return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while updating a user plant.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Updates only the custom name of a plant in a user's garden.
    /// </summary>
    /// <param name="userPlantId">The identifier of the user's plant entry.</param>
    /// <param name="newName">The new custom name for the plant.</param>
    /// <returns>True if the name update was successful, or an error.</returns>
    public async Task<Result<bool>> UpdateUserPlantName(Guid userPlantId, string newName)
    {
         try
         {
             if (string.IsNullOrEmpty(newName))
             {
                 return Error.Validation("New name cannot be empty.");
             }

             var updateResult = await _userPlantRepository.UpdateRangeAsync(
                 filter: up => up.UserPlantId == userPlantId, 
                 updateExpression: calls => calls.SetProperty(up => up.UserPlantName, newName) 
             );

             if (!updateResult.IsError)
             {
                  if (updateResult.Value == 0)
                  {
                       return Error.NotFound($"User plant entry with ID {userPlantId} not found.");
                  }
                  return true;
             }

             return updateResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while updating the user plant name.");
             return Error.Unexpected();
         }
    }
}
