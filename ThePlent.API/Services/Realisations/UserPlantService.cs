using Microsoft.Extensions.Logging;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations
{
    public class UserPlantService : IUserPlantService
    {
        private readonly ILogger<UserPlantService> _logger;
        private readonly IGenericRepository<UserPlant> _userPlantRepository;

        public UserPlantService(ILogger<UserPlantService> logger, IGenericRepository<UserPlant> userPlantRepository)
        {
            _logger = logger;
            _userPlantRepository = userPlantRepository;
        }

        public async Task<Result<IEnumerable<UserPlant>>> GetUserPlants(Guid userId)
        {
            try
            {
                var result = await _userPlantRepository.GetListAsync<UserPlant>(up => up.UserId == userId);

                if (!result.IsError)
                    return result;

                return result.Error;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching user plants.");
                return Error.Unexpected();
            }
        }
    }
}
