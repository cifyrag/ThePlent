using ThePlant.EF.Models;
using ThePlant.EF.Models.Enam;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Logs in a user with username and password.
    /// </summary>
    /// <param name="username">The user's username.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>The authenticated User object, or null if login fails.</returns>
    Task<Result<string>> LoginUser(string username, string password); 

        /// <summary>
        /// Registers a new user.
        /// </summary>+
        /// <param name="userData">The data for the new user.</param>
        /// <returns>The newly registered User object, or null if registration fails.</returns>
        Task<Result<User>> RegisterUser(User userData);

        /// <summary>
        /// Logs in an admin (could be an alternative login method like with a token).
        /// </summary>
        /// <param name="token">The login token.</param>
        Task<Result<string>> LoginAdmin(string username, string password);

        /// <summary>
        /// Logs in a user (could be an alternative login method like with a token).
        /// </summary>
        /// <param name="token">The login token.</param>
        Task<Result<User>> Login(string token); 

        /// <summary>
        /// Gets a user by their identifier.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>The User object, or null if not found.</returns>
        Task<Result<User>> GetUser(Guid userId); 
        
        /// <summary>
        /// Allows the user to choose their language preference.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="languageCode">The code for the chosen language.</param>
        Task<Result<Success>> ChooseLanguage(Guid userId, Language languageCode); 

        /// <summary>
        /// Updates the data for an existing user.
        /// </summary>
        /// <param name="userData">The updated user data.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<Result<bool>> UpdateUserData(User userData); 
        
        /// <summary>
        /// Sets the user's notification preferences.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="allow">True to allow notifications, false to disallow.</param>
        Task<Result<Success>> AllowNotifications(Guid userId, bool allow); 
}