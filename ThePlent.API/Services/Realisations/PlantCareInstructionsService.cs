using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

public class PlantCareInstructionsService: IPlantCareInstructionsService
{
    private readonly ILogger<PlantCareInstructionsService> _logger;

    public PlantCareInstructionsService(ILogger<PlantCareInstructionsService> logger)
    {
        _logger = logger;
    }

    public async Task<Result<PlantCareInstruction>> AddCareInstruction(PlantCareInstruction careInstructionData)
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

    public async Task<Result<PlantCareInstruction>> GetCareInstruction(Guid id)
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

    public async Task<Result<bool>> UpdateCareInstruction(PlantCareInstruction careInstructionData)
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

    public async Task<Result<bool>> RemoveCareInstruction(Guid instructionId)
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