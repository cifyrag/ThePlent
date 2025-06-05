using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces
{

    public interface IUserPlantService
    {
        Task<Result<IEnumerable<UserPlant>>> GetUserPlants(Guid userId);

        Task<Result<UserPlant>> AddUserPlant(UserPlant userPlant);

        Task<Result<bool>> DeleteUserPlant(Guid userPlantId);
    }

}
