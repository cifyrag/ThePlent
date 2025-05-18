using System.Runtime.InteropServices.JavaScript;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

public class UserGardenService: ІUserGardenService
{
    private readonly ILogger<UserGardenService> _logger;
    
    public UserGardenService(ILogger<UserGardenService> logger)
    {
        _logger = logger;
    }
    public async Task<Result<UserPlant>> AddUserPlant(Guid userId, Guid plantId)
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

    public async Task<Result<bool>> DeleteUserPlant(Guid userPlantId)
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
    
    public async Task<Result<bool>> RenameUserPlant(Guid userPlantId, string newName)
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

    public async Task<Result<bool>> UpdateUserPlant(UserPlant userPlantData)
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

    public async Task<Result<bool>> UpdateUserPlantName(Guid userPlantId, string newName)
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