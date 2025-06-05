using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;
using System.Linq;

namespace ThePlant.API.Services.Realisations;

/// <summary>
/// Implementation of the IPlantService interface for managing plant entries and care instructions.
/// </summary>
public class PlantService : IPlantService
{
    private readonly ILogger<PlantService> _logger;
    private readonly IGenericRepository<Plant> _plantRepository; 
    private readonly IGenericRepository<PlantCareInstruction> _plantCareInstructionRepository;
    private readonly IGenericRepository<PlantImage> _plantImageRepository;

    /// <summary>
    /// Initializes a new instance of the PlantService class.
    /// </summary>
    /// <param name="logger">The logger for the service.</param>
    /// <param name="plantRepository">The generic repository for Plant.</param>
    /// <param name="plantCareInstructionRepository">The generic repository for PlantCareInstruction.</param>
    public PlantService(
        ILogger<PlantService> logger,
        IGenericRepository<Plant> plantRepository,
        IGenericRepository<PlantCareInstruction> plantCareInstructionRepository,
        IGenericRepository<PlantImage> plantImageRepository)
    {
        _logger = logger;
        _plantRepository = plantRepository;
        _plantCareInstructionRepository = plantCareInstructionRepository;
        _plantImageRepository = plantImageRepository;
    }

    /// <summary>
    /// Adds a new plant.
    /// </summary>
    /// <param name="plantData">The data for the new plant.</param>
    /// <returns>The newly added Plant object, or an error.</returns>
    public async Task<Result<Plant>> AddPlant(Plant plantData)
    {
         try
         {
             if (plantData == null)
             {
                 return Error.Validation("Plant data cannot be null.");
             }

             var result = await _plantRepository.AddAsync(plantData);

             if (!result.IsError)
             {
                 return result.Value;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while adding a plant.");
             return Error.Unexpected();
         }
    }

    public async Task<Result<IEnumerable<PlantImage>>> GetPlantImages(Guid plantId)
    {
        try
        {
            // First, check if the plant actually exists to provide a more specific error
            var plantExists = await _plantRepository.GetSingleAsync<Plant>(p => p.PlantId == plantId);
            if (plantExists.IsError)
            {
                return plantExists.Error; // Propagate repository errors
            }
            if (plantExists.Value == null)
            {
                return Error.NotFound($"Plant with ID {plantId} not found.");
            }

            // Now, get the images for that plant
            var imagesResult = await _plantImageRepository.GetListAsync<PlantImage>(pi => pi.PlantId == plantId);

            if (!imagesResult.IsError)
            {
                return imagesResult; // Returns the Result<IEnumerable<PlantImage>>
            }

            return imagesResult.Error; // Propagate repository errors
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching images for plant ID {plantId}.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Updates an existing plant.
    /// </summary>
    /// <param name="plantData">The updated plant data.</param>
    /// <returns>True if the update was successful, or an error.</returns>
    public async Task<Result<bool>> UpdatePlant(Plant plantData)
    {
         try
         {
             if (plantData == null)
             {
                 return Error.Validation("Valid plant data is required for update.");
             }

             var result = await _plantRepository.UpdateAsync(plantData);

             if (!result.IsError)
             {
                 return true;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while updating a plant.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Gets a single plant by its identifier.
    /// </summary>
    /// <param name="plantId">The identifier of the plant.</param>
    /// <returns>The Plant object, or a 404 Not Found error.</returns>
    public async Task<Result<Plant>> GetPlant(Guid plantId)
    {
         try
         {
             var result = await _plantRepository.GetSingleAsync<Plant>(p => p.PlantId == plantId);

             if (!result.IsError)
             {
                 if (result?.Value == null)
                 {
                     return Error.NotFound($"Plant with ID {plantId} not found.");
                 }
                 return result.Value;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while getting a plant by ID.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Gets a collection of plants.
    /// </summary>
    /// <returns>An enumerable collection of Plant objects, or an error.</returns>
    public async Task<Result<System.Collections.Generic.IEnumerable<Plant>>> GetPlants()
    {
         try
         {
             var result = await _plantRepository.GetListAsync<Plant>();

             if (!result.IsError)
             {
                 return result;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while getting plants.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Removes a plant by its identifier.
    /// </summary>
    /// <param name="plantId">The identifier of the plant to remove.</param>
    /// <returns>True if the removal was successful, or an error.</returns>
    public async Task<Result<bool>> RemovePlant(Guid plantId)
    {
         try
         {
             var plantToRemoveResult = await _plantRepository.GetSingleAsync<Plant>(p => p.PlantId == plantId);

             if (plantToRemoveResult.IsError)
             {
                 return plantToRemoveResult.Error;
             }

             if (plantToRemoveResult?.Value == null)
             {
                 return Error.NotFound($"Plant with ID {plantId} not found.");
             }

             var removeResult = await _plantRepository.RemoveAsync(plantToRemoveResult.Value);

             if (!removeResult.IsError)
             {
                 return true;
             }

             return removeResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while removing a plant.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Retrieves a collection of plants, possibly for display purposes.
    /// </summary>
    /// <returns>An enumerable collection of Plant objects, or an error.</returns>
    public async Task<Result<System.Collections.Generic.IEnumerable<Plant>>> ShowPlants()
    {
         try
         {
             var result = await _plantRepository.GetListAsync<Plant>();

             if (!result.IsError)
             {
                 return result;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while showing plants.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Gets the care instructions for a specific plant.
    /// </summary>
    /// <param name="plantId">The identifier of the plant.</param>
    /// <returns>The PlantCareInstruction object for the plant, or a 404 Not Found error.</returns>
    public async Task<Result<PlantCareInstruction>> ViewPlantCareInstructions(Guid plantId)
    {
         try
         {
             var result = await _plantCareInstructionRepository.GetSingleAsync<PlantCareInstruction>(pci => pci.PlantId == plantId);

             if (!result.IsError)
             {
                 if (result?.Value == null)
                 {
                     return Error.NotFound($"Plant care instructions not found for Plant ID {plantId}.");
                 }
                 return result.Value;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while viewing plant care instructions.");
             return Error.Unexpected();
         }
    }
}
