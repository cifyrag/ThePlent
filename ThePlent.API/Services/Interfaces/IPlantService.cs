using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces;

public interface IPlantService
{
    /// <summary>
        /// Adds a new plant.
        /// </summary>
        /// <param name="plantData">The data for the new plant.</param>
        /// <returns>The newly added Plant object, or null if adding fails.</returns>
        Task<Result<Plant>> AddPlant(Plant plantData); 
    
        /// <summary>
        /// Updates an existing plant.
        /// </summary>
        /// <param name="plantData">The updated plant data.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<Result< bool>> UpdatePlant(Plant plantData);
        
        /// <summary>
        /// Gets a single plant by its identifier.
        /// </summary>
        /// <param name="plantId">The identifier of the plant.</param>
        /// <returns>The Plant object, or null if not found.</returns>
        Task<Result<Plant>> GetPlant(int plantId);
        
        /// <summary>
        /// Gets a collection of plants.
        /// </summary>
        /// <remarks>
        /// This could potentially take parameters for filtering,
        /// although a specific FilterPlants method is shown as non-public.
        /// </remarks>
        /// <returns>An enumerable collection of Plant objects.</returns>
        Task<Result<System.Collections.Generic.IEnumerable<Plant>>> GetPlants(); 
        
        /// <summary>
        /// Removes a plant by its identifier.
        /// </summary>
        /// <param name="plantId">The identifier of the plant to remove.</param>
        /// <returns>True if the removal was successful, false otherwise.</returns>
        Task<Result<bool>> RemovePlant(int plantId); 

        /// <summary>
        /// Retrieves a collection of plants, possibly for display purposes.
        /// </summary>
        /// <returns>An enumerable collection of Plant objects.</returns>
        Task<Result<System.Collections.Generic.IEnumerable<Plant>>> ShowPlants(); 

        /// <summary>
        /// Gets the care instructions for a specific plant.
        /// </summary>
        /// <param name="plantId">The identifier of the plant.</param>
        /// <returns>The PlantCareInstructions object for the plant, or null if not found.</returns>
        Task<Result<PlantCareInstruction>> ViewPlantCareInstructions(int plantId); 
    }