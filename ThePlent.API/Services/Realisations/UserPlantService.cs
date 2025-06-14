﻿using Microsoft.Extensions.Logging;
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

        public UserPlantService(ILogger<UserPlantService> logger, IGenericRepository<UserPlant> userPlantRepository)
        {
            _logger = logger;
            _userPlantRepository = userPlantRepository;
        }

        public async Task<Result<IEnumerable<UserPlant>>> GetUserPlants(Guid userId)
        {
            try
            {
                var result = await _userPlantRepository.GetListAsync<UserPlant>(up => up.UserId == userId);

                if (!result.IsError)
                    return result;

                return result.Error;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user plants.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<UserPlant>> AddUserPlant(UserPlant userPlant)
        {
            try
            {
                if (userPlant == null)
                    return Error.Validation("UserPlant data cannot be null.");

                var result = await _userPlantRepository.AddAsync(userPlant);

                if (!result.IsError)
                    return result.Value;

                return result.Error;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding user plant.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<UserPlant>> UpdateUserPlant(UserPlant userPlant) // Added Update method implementation
        {
            try
            {
                if (userPlant == null)
                    return Error.Validation("UserPlant data cannot be null.");

                var existingUserPlantResult = await _userPlantRepository.GetSingleAsync<UserPlant>(up => up.UserPlantId == userPlant.UserPlantId);

                if (existingUserPlantResult.IsError)
                    return existingUserPlantResult.Error;

                if (existingUserPlantResult.Value == null)
                    return Error.NotFound($"UserPlant with ID {userPlant.UserPlantId} not found.");

                // Update the existing entity with the new values
                // You might need to manually map properties or use a library like AutoMapper
                // For simplicity, directly update the properties if you have a full entity
                // If IGenericRepository.UpdateAsync expects the full entity, you'd update existingUserPlantResult.Value
                // For example: existingUserPlantResult.Value.SomeProperty = userPlant.SomeProperty;

                var updateResult = await _userPlantRepository.UpdateAsync(userPlant);

                if (!updateResult.IsError)
                    return updateResult.Value;

                return updateResult.Error;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user plant.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<bool>> DeleteUserPlant(Guid userPlantId)
        {
            try
            {
                var userPlantResult = await _userPlantRepository.GetSingleAsync<UserPlant>(up => up.UserPlantId == userPlantId);

                if (userPlantResult.IsError)
                    return userPlantResult.Error;

                if (userPlantResult.Value == null)
                    return Error.NotFound($"UserPlant with ID {userPlantId} not found.");

                var deleteResult = await _userPlantRepository.RemoveAsync(userPlantResult.Value);

                if (!deleteResult.IsError)
                    return true;

                return deleteResult.Error;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting user plant.");
                return Error.Unexpected();
            }
        }
    }
}