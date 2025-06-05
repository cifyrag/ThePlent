using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces
{
    public interface IPlantImagesService
    {
        Task<Result<PlantImage>> AddImage(PlantImage imageData);
        Task<Result<PlantImage>> GetImage(Guid id);
        Task<Result<bool>> UpdateImage(PlantImage imageData);
        Task<Result<bool>> RemoveImage(Guid imageId);
    }
}
