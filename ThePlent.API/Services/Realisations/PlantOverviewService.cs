using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

public class PlantOverviewService: IPlantOverviewService
{
    private readonly ILogger<PlantOverviewService> _logger;

    public PlantOverviewService(ILogger<PlantOverviewService> logger)
    {
        _logger = logger;
    }

    public async Task<Result<PlantOverview>> AddPlant_Overview(PlantOverview plantOverviewData)
    {
         try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");

            return Error.Unexpected();
        }
    }

    public async Task<Result<PlantOverview>> GetPlant_Overview(int overviewId)
    {
         try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");

            return Error.Unexpected();
        }
    }

    public async Task<Result<bool>> DeletePlant_Overview(int overviewId)
    {
         try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");

            return Error.Unexpected();
        }
    }
}