using ThePlant.EF.Models;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces;

public interface IRemindersService
{
    /// <summary>
    /// Marks a specific reminder as done for a user.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="reminderId">The identifier of the reminder to mark as done.</param>
    /// <returns>True if the reminder was marked as done successfully, false otherwise.</returns>
    Task<Result<bool>> MarkAsDone(Guid userId, Guid reminderId);

    /// <summary>
    /// Imports a reminder to a user's calendar.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="reminderId">The identifier of the reminder to import.</param>
    /// <param name="calendarId">The identifier of the calendar to import to (optional).</param>
    /// <returns>True if the import was successful, false otherwise.</returns>
    Task<Result<bool>> ImportToCalendar(Guid userId, Guid reminderId, string calendarId = null);

    /// <summary>
    /// Gets all reminders for a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>An enumerable collection of Reminder objects for the user.</returns>
    Task<Result<System.Collections.Generic.IEnumerable<Reminder>>> GetUserReminders(Guid userId);

    /// <summary>
    /// Creates a new reminder for a user.
    /// </summary>
    /// <param name="reminderData">The data for the new reminder.</param>
    /// <returns>The newly created Reminder object, or null if creation fails.</returns>
    Task<Result<Reminder>> CreateReminder(Reminder reminderData);

    /// <summary>
    /// Deletes a reminder by its identifier.
    /// </summary>
    /// <param name="reminderId">The identifier of the reminder to delete.</param>
    /// <returns>True if the deletion was successful, false otherwise.</returns>
    Task<Result<bool>> DeleteReminder(Guid reminderId);

    /// <summary>
    /// Gets upcoming reminders for a user within a specified timeframe.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="timeframeInDays">The number of upcoming days to check (optional).</param>
    /// <returns>An enumerable collection of upcoming Reminder objects for the user.</returns>
    Task<Result<System.Collections.Generic.IEnumerable<Reminder>>> GetUpcomingReminders(Guid userId, int timeframeInDays = 7);

    /// <summary>
    /// Updates an existing reminder.
    /// </summary>
    /// <param name="reminderData">The updated reminder data.</param>
    /// <returns>True if the update was successful, false otherwise.</returns>
    Task<Result<bool>> UpdateReminder(Reminder reminderData);

    /// <summary>
    /// Gets all reminders associated with a specific UserPlant ID.
    /// </summary>
    /// <param name="userPlantId">The identifier of the UserPlant.</param>
    /// <returns>An enumerable collection of Reminder objects for the UserPlant.</returns>
    Task<Result<System.Collections.Generic.IEnumerable<Reminder>>> GetRemindersByUserPlantId(Guid userPlantId);
}