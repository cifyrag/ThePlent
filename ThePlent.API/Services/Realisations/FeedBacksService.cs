
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

/// <summary>
/// Implementation of the IFeedBacksService interface for managing feedback entries.
/// </summary>
public class FeedBacksService : IFeedBacksService
{
    private readonly ILogger<FeedBacksService> _logger;
    private readonly IGenericRepository<Feedback> _feedbackRepository; 

    /// <summary>
    /// Initializes a new instance of the FeedBacksService class.
    /// </summary>
    /// <param name="logger">The logger for the service.</param>
    /// <param name="feedbackRepository">The generic repository for Feedback.</param>
    public FeedBacksService(ILogger<FeedBacksService> logger, IGenericRepository<Feedback> feedbackRepository)
    {
        _logger = logger;
        _feedbackRepository = feedbackRepository;
    }

    /// <summary>
    /// Adds a new feedback entry.
    /// </summary>
    /// <param name="feedback">The feedback object to add.</param>
    /// <returns>A Success object indicating the action was successful, or an error.</returns>
    public async Task<Result<Success>> AddFeedBack(Feedback feedback)
    {
         try
         {
             if (feedback == null)
             {
                 return Error.Validation("Feedback data cannot be null.");
             }

             var result = await _feedbackRepository.AddAsync(feedback);

             if (!result.IsError)
             {
                 return Success.Instance;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while adding feedback.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Gets a single feedback entry by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the feedback.</param>
    /// <returns>The feedback object, or a 404 Not Found error.</returns>
    public async Task<Result<Feedback>> GetFeedBack(Guid id)
    {
         try
         {
             var result = await _feedbackRepository.GetSingleAsync<Feedback>(f => f.FeedbackId == id); 

             if (!result.IsError)
             {
                 if (result?.Value == null)
                 {
                     return Error.NotFound($"Feedback with ID {id} not found.");
                 }
                 return result.Value;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while getting feedback by ID.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Gets a collection of feedback entries.
    /// </summary>
    /// <returns>An enumerable collection of feedback objects, or an error.</returns>
    public async Task<Result<System.Collections.Generic.IEnumerable<Feedback>>> GetFeedBacks()
    {
         try
         {
             var result = await _feedbackRepository.GetListAsync<Feedback>();

             if (!result.IsError)
             {
                 return result;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while getting feedback entries.");
             return Error.Unexpected();
         }
    }
}
