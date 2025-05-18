
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

/// <summary>
/// Implementation of the IPlantCareInstructionsService interface for managing plant care instructions.
/// </summary>
public class PlantCareInstructionsService : IPlantCareInstructionsService
{
    private readonly ILogger<PlantCareInstructionsService> _logger;
    private readonly IGenericRepository<PlantCareInstruction> _plantCareInstructionRepository; 

    /// <summary>
    /// Initializes a new instance of the PlantCareInstructionsService class.
    /// </summary>
    /// <param name="logger">The logger for the service.</param>
    /// <param name="plantCareInstructionRepository">The generic repository for PlantCareInstruction.</param>
    public PlantCareInstructionsService(ILogger<PlantCareInstructionsService> logger, IGenericRepository<PlantCareInstruction> plantCareInstructionRepository)
    {
        _logger = logger;
        _plantCareInstructionRepository = plantCareInstructionRepository;
    }

    /// <summary>
    /// Adds a new plant care instruction.
    /// </summary>
    /// <param name="careInstructionData">The data for the new care instruction.</param>
    /// <returns>The newly added PlantCareInstruction object, or an error.</returns>
    public async Task<Result<PlantCareInstruction>> AddCareInstruction(PlantCareInstruction careInstructionData)
    {
         try
         {
             if (careInstructionData == null)
             {
                 return Error.Validation("Plant care instruction data cannot be null.");
             }

             var result = await _plantCareInstructionRepository.AddAsync(careInstructionData);

             if (!result.IsError)
             {
                 return result.Value;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while adding a plant care instruction.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Gets a single plant care instruction by its identifier or associated plant identifier.
    /// </summary>
    /// <param name="id">The identifier of the care instruction or the plant.</param>
    /// <returns>The PlantCareInstruction object, or a 404 Not Found error.</returns>
    public async Task<Result<PlantCareInstruction>> GetCareInstruction(Guid id)
    {
         try
         {
             var result = await _plantCareInstructionRepository.GetSingleAsync<PlantCareInstruction>(pci => pci.PlantCareInstructionId == id); 
             
             if (!result.IsError)
             {
                 if (result?.Value == null)
                 {
                     return Error.NotFound($"Plant care instruction with ID {id} not found.");
                 }
                 return result.Value;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while getting a plant care instruction by ID.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Updates an existing plant care instruction.
    /// </summary>
    /// <param name="careInstructionData">The updated care instruction data.</param>
    /// <returns>True if the update was successful, or an error.</returns>
    public async Task<Result<bool>> UpdateCareInstruction(PlantCareInstruction careInstructionData)
    {
         try
         {
             if (careInstructionData == null)
             {
                 return Error.Validation("Valid plant care instruction data is required for update.");
             }

             var result = await _plantCareInstructionRepository.UpdateAsync(careInstructionData);

             if (!result.IsError)
             {
                 return true;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while updating a plant care instruction.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Removes a plant care instruction by its identifier.
    /// </summary>
    /// <param name="instructionId">The identifier of the care instruction to remove.</param>
    /// <returns>True if the removal was successful, or an error.</returns>
    public async Task<Result<bool>> RemoveCareInstruction(Guid instructionId)
    {
         try
         {
             var instructionToRemoveResult = await _plantCareInstructionRepository.GetSingleAsync<PlantCareInstruction>(
                 pci => pci.PlantCareInstructionId == instructionId); 

             if (instructionToRemoveResult.IsError)
             {
                 return instructionToRemoveResult.Error;
             }

             if (instructionToRemoveResult?.Value == null)
             {
                 return Error.NotFound($"Plant care instruction with ID {instructionId} not found.");
             }

             var removeResult = await _plantCareInstructionRepository.RemoveAsync(instructionToRemoveResult.Value);

             if (!removeResult.IsError)
             {
                 return true;
             }

             return removeResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while removing a plant care instruction.");
             return Error.Unexpected();
         }
    }
}
