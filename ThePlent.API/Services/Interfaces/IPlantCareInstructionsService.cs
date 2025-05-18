using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces;

public interface IPlantCareInstructionsService
{
    /// <summary>
    /// Adds a new plant care instruction.
    /// </summary>
    /// <param name="careInstructionData">The data for the new care instruction.</param>
    /// <returns>The newly added PlantCareInstruction object, or null if adding fails.</returns>
    Task<Result<PlantCareInstruction>> AddCareInstruction(PlantCareInstruction careInstructionData); 
    
    /// <summary>
    /// Gets a single plant care instruction by its identifier or associated plant identifier.
    /// </summary>
    /// <param name="id">The identifier of the care instruction or the plant.</param>
    /// <returns>The PlantCareInstruction object, or null if not found.</returns>
    Task<Result<PlantCareInstruction>> GetCareInstruction(Guid id); 
    
    /// <summary>
    /// Updates an existing plant care instruction.
    /// </summary>
    /// <param name="careInstructionData">The updated care instruction data.</param>
    /// <returns>True if the update was successful, false otherwise.</returns>
    Task<Result<bool>> UpdateCareInstruction(PlantCareInstruction careInstructionData); 
    
    /// <summary>
    /// Removes a plant care instruction by its identifier.
    /// </summary>
    /// <param name="instructionId">The identifier of the care instruction to remove.</param>
    /// <returns>True if the removal was successful, false otherwise.</returns>
    Task<Result<bool>> RemoveCareInstruction(Guid instructionId);
}