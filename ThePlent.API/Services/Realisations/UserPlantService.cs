using Microsoft.Extensions.Logging;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations
{
    public class UserPlantService : IUserPlantService
    {
        private readonly ILogger<UserPlantService> _logger;
        private readonly IGenericRepository<UserPlant> _userPlantRepository;
        private readonly IGenericRepository<Plant> _plantRepository;

        public UserPlantService(
            ILogger<UserPlantService> logger,
            IGenericRepository<UserPlant> userPlantRepository,
            IGenericRepository<Plant> plantRepository)
        {
            _logger = logger;
            _userPlantRepository = userPlantRepository;
            _plantRepository = plantRepository;
        }

        public async Task<Result<UserPlant>> GetUserPlant(Guid userPlantId)
        {
            try
            {
                var result = await _userPlantRepository.GetSingleAsync<UserPlant>(
                    filter: up => up.UserPlantId == userPlantId
                );

                if (result.IsError)
                    return result.Error;

                if (result.Value == null)
                    return Error.NotFound($"UserPlant with ID {userPlantId} not found.");

                return result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the UserPlant.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<List<UserPlant>>> GetUserPlantsByUserId(Guid userId)
        {
            try
            {
                var result = await _userPlantRepository.GetListAsync<UserPlant>(
                    filter: up => up.UserId == userId,
                    orderBy: null,
                    includes: null,
                    selector: up => up 
                );

                if (result.IsError)
                    return result.Error;

                return result.Value.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving UserPlants by userId.");
                return Error.Unexpected();
            }
        }


        public async Task<Result<UserPlant>> AddUserPlant(UserPlant userPlant)
        {
            try
            {
                if (userPlant == null)
                    return Error.Validation("UserPlant object must not be null.");

                var plantExists = await _plantRepository.ExistsAsync(p => p.PlantId == userPlant.PlantId);

                if (plantExists.IsError)
                    return plantExists.Error;

                if (!plantExists.Value)
                    return Error.NotFound($"Plant with ID {userPlant.PlantId} not found.");

                var addResult = await _userPlantRepository.AddAsync(userPlant);

                return addResult.IsError ? addResult.Error : addResult.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a UserPlant.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<bool>> DeleteUserPlant(Guid userPlantId)
        {
            try
            {
                var userPlantResult = await _userPlantRepository.GetSingleAsync<UserPlant>(
                    up => up.UserPlantId == userPlantId
                );

                if (userPlantResult.IsError)
                    return userPlantResult.Error;

                if (userPlantResult.Value == null)
                    return Error.NotFound($"UserPlant with ID {userPlantId} not found.");

                var deleteResult = await _userPlantRepository.RemoveAsync(userPlantResult.Value);

                return deleteResult.IsError ? deleteResult.Error : true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a UserPlant.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<bool>> RenameUserPlant(Guid userPlantId, string newName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newName))
                    return Error.Validation("New name cannot be empty.");

                var updateResult = await _userPlantRepository.UpdateRangeAsync(
                    filter: up => up.UserPlantId == userPlantId,
                    updateExpression: updates => updates.SetProperty(up => up.UserPlantName, newName)
                );

                if (updateResult.IsError)
                    return updateResult.Error;

                if (updateResult.Value == 0)
                    return Error.NotFound($"UserPlant with ID {userPlantId} not found.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while renaming the UserPlant.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<bool>> UpdateUserPlant(UserPlant userPlantData)
        {
            try
            {
                if (userPlantData == null)
                    return Error.Validation("UserPlant data must not be null.");

                var result = await _userPlantRepository.UpdateAsync(userPlantData);

                return result.IsError ? result.Error : true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the UserPlant.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<bool>> UpdateUserPlantName(Guid userPlantId, string newName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newName))
                    return Error.Validation("New name cannot be empty.");

                var result = await _userPlantRepository.UpdateRangeAsync(
                    filter: up => up.UserPlantId == userPlantId,
                    updateExpression: update => update.SetProperty(up => up.UserPlantName, newName)
                );

                if (result.IsError)
                    return result.Error;

                if (result.Value == 0)
                    return Error.NotFound($"UserPlant with ID {userPlantId} not found.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the UserPlant name.");
                return Error.Unexpected();
            }
        }
    }
}
