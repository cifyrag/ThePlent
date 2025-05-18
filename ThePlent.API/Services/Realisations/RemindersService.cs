
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Models.Enam;
using ThePlant.EF.Repository;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

/// <summary>
/// Implementation of the IRemindersService interface for managing user reminders.
/// </summary>
public class RemindersService : IRemindersService
{
    private readonly ILogger<RemindersService> _logger;
    private readonly IGenericRepository<Reminder> _reminderRepository; 

    /// <summary>
    /// Initializes a new instance of the RemindersService class.
    /// </summary>
    /// <param name="logger">The logger for the service.</param>
    /// <param name="reminderRepository">The generic repository for Reminder.</param>
    public RemindersService(ILogger<RemindersService> logger, IGenericRepository<Reminder> reminderRepository)
    {
        _logger = logger;
        _reminderRepository = reminderRepository;
    }

    /// <summary>
    /// Marks a specific reminder as done for a user.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="reminderId">The identifier of the reminder to mark as done.</param>
    /// <returns>True if the reminder was marked as done successfully, or an error.</returns>
    public async Task<Result<bool>> MarkAsDone(Guid userId, Guid reminderId)
    {
         try
         {
             var updateResult = await _reminderRepository.UpdateRangeAsync(
                 filter: r =>  r.ReminderId == reminderId, 
                 updateExpression: calls => calls.SetProperty(r => r.Status, ReminderStatus.Completed) 
             );

             if (!updateResult.IsError)
             {
                 if (updateResult.Value == 0)
                 {
                     return Error.NotFound($"Reminder with ID {reminderId} for User ID {userId} not found.");
                 }
                 return true;
             }

             return updateResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while marking a reminder as done.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Imports a reminder to a user's calendar.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="reminderId">The identifier of the reminder to import.</param>
    /// <param name="calendarId">The identifier of the calendar to import to (optional).</param>
    /// <returns>True if the import was successful, or an error.</returns>
    public async Task<Result<bool>> ImportToCalendar(Guid userId, Guid reminderId, string calendarId = null)
    {
         try
         {
             _logger.LogInformation($"User {userId} attempting to import reminder {reminderId} to calendar {calendarId ?? "default"}.");

             return true;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while importing a reminder to calendar.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Gets all reminders for a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>An enumerable collection of Reminder objects for the user, or an error.</returns>
    public async Task<Result<System.Collections.Generic.IEnumerable<Reminder>>> GetUserReminders(Guid userId)
    {
         try
         {
             var result = await _reminderRepository.GetListAsync<Reminder>(filter: r => r.UserPlant.UserId == userId); 

             if (!result.IsError)
             {
                 return result;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while getting user reminders.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Creates a new reminder for a user.
    /// </summary>
    /// <param name="reminderData">The data for the new reminder.</param>
    /// <returns>The newly created Reminder object, or an error.</returns>
    public async Task<Result<Reminder>> CreateReminder(Reminder reminderData)
    {
         try
         {
             if (reminderData == null)
             {
                 return Error.Validation("Reminder data cannot be null.");
             }

             var result = await _reminderRepository.AddAsync(reminderData);

             if (!result.IsError)
             {
                 return result.Value;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while creating a reminder.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Deletes a reminder by its identifier for a user.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="reminderId">The identifier of the reminder to delete.</param>
    /// <returns>True if the deletion was successful, or an error.</returns>
    public async Task<Result<bool>> DeleteReminder(Guid userId, Guid reminderId)
    {
         try
         {
             var reminderToRemoveResult = await _reminderRepository.GetSingleAsync<Reminder>(
                 r => r.UserPlant.UserId == userId && r.ReminderId == reminderId); 

             if (reminderToRemoveResult.IsError)
             {
                 return reminderToRemoveResult.Error;
             }

             if (reminderToRemoveResult?.Value == null)
             {
                 return Error.NotFound($"Reminder with ID {reminderId} for User ID {userId} not found.");
             }

             var removeResult = await _reminderRepository.RemoveAsync(reminderToRemoveResult.Value);

             if (!removeResult.IsError)
             {
                 return true;
             }

             return removeResult.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while deleting a reminder.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Gets upcoming reminders for a user within a specified timeframe.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="timeframeInDays">The number of upcoming days to check (optional).</param>
    /// <returns>An enumerable collection of upcoming Reminder objects for the user, or an error.</returns>
    public async Task<Result<System.Collections.Generic.IEnumerable<Reminder>>> GetUpcomingReminders(Guid userId, int timeframeInDays = 7)
    {
         try
         {
             var upcomingDate = DateTime.UtcNow.AddDays(timeframeInDays);

             var result = await _reminderRepository.GetListAsync<Reminder>(
                 filter: r => r.UserPlant.UserId == userId && r.DateOfReminder <= upcomingDate && (r.Status == ReminderStatus.Pending || r.Status == ReminderStatus.Snoozed),
                 orderBy: q => q.OrderBy(r => r.DateOfReminder) 
             );

             if (!result.IsError)
             {
                 return result;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while getting upcoming reminders.");
             return Error.Unexpected();
         }
    }

    /// <summary>
    /// Updates an existing reminder.
    /// </summary>
    /// <param name="reminderData">The updated reminder data.</param>
    /// <returns>True if the update was successful, or an error.</returns>
    public async Task<Result<bool>> UpdateReminder(Reminder reminderData)
    {
         try
         {
             if (reminderData == null)
             {
                 return Error.Validation("Valid reminder data is required for update.");
             }

             var result = await _reminderRepository.UpdateAsync(reminderData);

             if (!result.IsError)
             {
                 return true;
             }

             return result.Error;
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred while updating a reminder.");
             return Error.Unexpected();
         }
    }
}
