
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

/// <summary>
/// Implementation of the IPlantOverviewService interface for managing plant overview entries.
/// </summary>
public class PlantOverviewService : IPlantOverviewService
{
    private readonly ILogger<PlantOverviewService> _logger;
    private readonly IGenericRepository<PlantOverview> _plantOverviewRepository; 

    /// <summary>
    /// Initializes a new instance of the PlantOverviewService class.
    /// </summary>
    /// <param name="logger">The logger for the service.</param>
    /// <param name="plantOverviewRepository">The generic repository for PlantOverview.</param>
    public PlantOverviewService(ILogger<PlantOverviewService> logger, IGenericRepository<PlantOverview> plantOverviewRepository)
    {
        _logger = logger;
        _plantOverviewRepository = plantOverviewRepository;
    }

    /// <summary>
    /// Adds a new plant overview entry.
    /// </summary>
    /// <param name="plantOverviewData">The data for the new plant overview.</param>
    /// <returns>The newly added PlantOverview object, or an error.</returns>
    public async Task<Result<PlantOverview>> AddPlant_Overview(PlantOverview plantOverviewData)
    {
         try
         {
             if (plantOverviewData == null)
             {
                 return Error.Validation("Plant overview data cannot be null.");
             }

             var result = await _plantOverviewRepository.AddAsync(plantOverviewData);

             if (!result.IsError)
             {
                 return result.Value;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while adding a plant overview.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Gets a single plant overview entry by its identifier.
    /// </summary>
    /// <param name="overviewId">The identifier of the plant overview.</param>
    /// <returns>The PlantOverview object, or a 404 Not Found error.</returns>
    public async Task<Result<PlantOverview>> GetPlant_Overview(Guid overviewId)
    {
         try
         {
             var result = await _plantOverviewRepository.GetSingleAsync<PlantOverview>(po => po.PlantOverviewId == overviewId);

             if (!result.IsError)
             {
                 if (result?.Value == null)
                 {
                     return Error.NotFound($"Plant overview with ID {overviewId} not found.");
                 }
                 return result.Value;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while getting a plant overview by ID.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Deletes a plant overview entry by its identifier.
    /// </summary>
    /// <param name="overviewId">The identifier of the plant overview to delete.</param>
    /// <returns>True if the deletion was successful, or an error.</returns>
    public async Task<Result<bool>> DeletePlant_Overview(Guid overviewId)
    {
         try
         {
             var overviewToRemoveResult = await _plantOverviewRepository.GetSingleAsync<PlantOverview>(
                 po => po.PlantOverviewId == overviewId);

             if (overviewToRemoveResult.IsError)
             {
                 return overviewToRemoveResult.Error;
             }

             if (overviewToRemoveResult?.Value == null)
             {
                 return Error.NotFound($"Plant overview with ID {overviewId} not found.");
             }

             var removeResult = await _plantOverviewRepository.RemoveAsync(overviewToRemoveResult.Value);

             if (!removeResult.IsError)
             {
                 return true;
             }

             return removeResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while deleting a plant overview.");
             return Error.Unexpected();
         }
    }
}
