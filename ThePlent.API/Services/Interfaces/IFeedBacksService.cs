using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces;

public interface IFeedBacksService
{
    /// <summary>
    /// Adds a new feedback entry.
    /// </summary>
    /// <param name="feedback">The feedback object to add.</param>
    Task<Result<Success>> AddFeedBack(Feedback feedback);

    /// <summary>
    /// Gets a single feedback entry by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the feedback.</param>
    /// <returns>The feedback object, or null if not found.</returns>
    Task<Result<Feedback>> GetFeedBack(Guid id); 

    /// <summary>
    /// Gets a collection of feedback entries.
    /// </summary>
    /// <returns>An enumerable collection of feedback objects.</returns>
    Task<Result<System.Collections.Generic.IEnumerable<Feedback>>> GetFeedBacks(); 
}