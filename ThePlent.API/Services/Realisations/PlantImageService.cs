using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;
using System.Linq; // Add this using directive for .Any()

namespace ThePlant.API.Services.Realisations
{
    public class PlantImagesService : IPlantImagesService
    {
        private readonly ILogger<PlantImagesService> _logger;
        private readonly IGenericRepository<PlantImage> _plantImageRepository;

        public PlantImagesService(ILogger<PlantImagesService> logger, IGenericRepository<PlantImage> plantImageRepository)
        {
            _logger = logger;
            _plantImageRepository = plantImageRepository;
        }

        public async Task<Result<PlantImage>> AddImage(PlantImage imageData)
        {
            try
            {
                if (imageData == null)
                    return Error.Validation("Image data cannot be null.");

                var result = await _plantImageRepository.AddAsync(imageData);

                return !result.IsError ? result.Value : result.Error;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding plant image.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<PlantImage>> GetImage(Guid id)
        {
            try
            {
                var result = await _plantImageRepository.GetSingleAsync<PlantImage>(img => img.PlantImageId == id);

                if (result.IsError)
                {
                    return result.Error;
                }

                if (result.Value == null)
                {
                    return Error.NotFound($"Image with ID {id} not found.");
                }

                return result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving plant image.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<IEnumerable<PlantImage>>> GetImagesByPlantId(Guid plantId)
        {
            try
            {
                var result = await _plantImageRepository.GetListAsync<PlantImage>(img => img.PlantId == plantId);

                if (result.IsError)
                {
                    return result.Error;
                }

                // Check if the result.Value is null or empty after a successful repository call
                // If you want to return NotFound error for no images found, keep this check.
                // Otherwise, an empty IEnumerable<PlantImage> is also a valid success case.
                if (result.Value == null || !result.Value.Any())
                {
                    return Error.NotFound($"No images found for Plant with ID {plantId}.");
                }

                // Corrected line: Return the 'result' object itself, as it's already of type Result<IEnumerable<PlantImage>>
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving plant images by Plant ID.");
                return Error.Unexpected();
            }
        }


        public async Task<Result<bool>> UpdateImage(PlantImage imageData)
        {
            try
            {
                if (imageData == null)
                    return Error.Validation("Valid image data is required for update.");

                var result = await _plantImageRepository.UpdateAsync(imageData);

                return !result.IsError ? true : result.Error;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating plant image.");
                return Error.Unexpected();
            }
        }

        public async Task<Result<bool>> RemoveImage(Guid imageId)
        {
            try
            {
                var result = await _plantImageRepository.GetSingleAsync<PlantImage>(img => img.PlantImageId == imageId);

                if (result.IsError)
                    return result.Error;

                if (result?.Value == null)
                    return Error.NotFound($"Image with ID {imageId} not found.");

                var removeResult = await _plantImageRepository.RemoveAsync(result.Value);

                return !removeResult.IsError ? true : removeResult.Error;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing plant image.");
                return Error.Unexpected();
            }
        }
    }
}