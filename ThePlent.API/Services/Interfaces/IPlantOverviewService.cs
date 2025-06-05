using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces;

public interface IPlantOverviewService
{
    /// <summary>
    /// Adds a new plant overview entry.
    /// </summary>
    /// <param name="plantOverviewData">The data for the new plant overview.</param>
    /// <returns>The newly added PlantOverview object, or null if adding fails.</returns>
    Task<Result<PlantOverview>> AddPlant_Overview(PlantOverview plantOverviewData); 
    
    /// <summary>
    /// Gets a single plant overview entry by its identifier.
    /// </summary>
    /// <param name="overviewId">The identifier of the plant overview.</param>
    /// <returns>The PlantOverview object, or null if not found.</returns>
    Task<Result<PlantOverview>> GetPlant_Overview(Guid overviewId); 
    
    /// <summary>
    /// Deletes a plant overview entry by its identifier.
    /// </summary>
    /// <param name="overviewId">The identifier of the plant overview to delete.</param>
    /// <returns>True if the deletion was successful, false otherwise.</returns>
    Task<Result<bool>> DeletePlant_Overview(Guid overviewId);

    /// <summary>
    /// Gets all plant overviews associated with a specific Plant ID.
    /// </summary>
    /// <param name="plantId">The identifier of the Plant.</param>
    /// <returns>An enumerable collection of PlantOverview objects for the Plant.</returns>
    Task<Result<System.Collections.Generic.IEnumerable<PlantOverview>>> GetPlantOverviewsByPlantId(Guid plantId);
}