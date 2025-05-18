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
    public async Task<Result<UserPlant>> AddUserPlant(int userId, int plantId)
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

    public async Task<Result<bool>> DeleteUserPlant(int userPlantId)
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

    public async Task<Result<bool>> RenameUserPlant(int userPlantId, string newName)
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

    public async Task<Result<bool>> UpdateUserPlantName(int userPlantId, string newName)
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