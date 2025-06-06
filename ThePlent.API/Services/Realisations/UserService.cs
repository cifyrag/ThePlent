
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Services.Notifications;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Models.Enam;
using ThePlant.EF.Repository;
using ThePlant.EF.Settings;
using ThePlant.EF.Utils;

namespace ThePlant.API.Services.Realisations;

/// <summary>
/// Implementation of the IUserService interface for managing user accounts and profiles.
/// </summary>
public class UserService : IUserService
{
    private readonly TimeSpan expiration = TimeSpan.FromDays(365);
    private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SigningCredentials _signingCredentials;
    private readonly ILogger<UserService> _logger;
    private readonly IGenericRepository<User> _userRepository; // Repository for User
    private const int SALT_SIZE = 128 / 8;
    private const int KEY_SIZE = 256 / 8;
    private const int ITERATIONS = 1000;
    private const char DELIMITER = ';';
    private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA256;

    /// <summary>
    /// Initializes a new instance of the UserService class.
    /// </summary>
    /// <param name="logger">The logger for the service.</param>
    /// <param name="userRepository">The generic repository for User.</param>
    public UserService(ILogger<UserService> logger, IGenericRepository<User> userRepository, IOptions<JWTSettings> jwtSettings)
    {
        _logger = logger;
        _userRepository = userRepository;
        _signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.IssuerSigningKey)),
            SecurityAlgorithms.HmacSha256
        );
        _issuer = jwtSettings.Value.ValidIssuer;
        _audience = jwtSettings.Value.ValidAudience;
    }

    /// <summary>
    /// Logs in a user with username and password.
    /// </summary>
    /// <param name="username">The user's username.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>The authenticated User object, or an error if login fails.</returns>
    public async Task<Result<string>> LoginUser(string username, string password)
    {
         try
         {
             if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
             {
                 return Error.Validation("UserName and password are required.");
             }

             var userResult = await _userRepository.GetSingleAsync<User>(u => u.UserName == username && u.IsAdmin == false); 

             if (userResult.IsError)
             {
                 return userResult.Error;
             }

             if (userResult?.Value == null)
             {
                 return Error.NotFound("User not found.");
             }

             bool isPasswordValid = VerifyPasswordHash(password, userResult.Value.Password); 
            
             if (!isPasswordValid)
             {
                 return Error.Unauthorized("Invalid credentials.");
             }

             return IssueToken(userResult.Value, Roles.User);
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "An error occurred during user login.");
             return Error.Unexpected();
         }
    }
    
    public async Task<Result<string>> LoginAdmin(string username, string password)
    {
        try
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Error.Validation("UserName and password are required.");
            }

            var userResult = await _userRepository.GetSingleAsync<User>(u => u.UserName == username && u.IsAdmin == true); 

            if (userResult.IsError)
            {
                return userResult.Error;
            }

            if (userResult?.Value == null)
            {
                return Error.NotFound("User not found.");
            }

            bool isPasswordValid = VerifyPasswordHash(password, userResult.Value.Password); 
            
            if (!isPasswordValid)
            {
                return Error.Unauthorized("Invalid credentials.");
            }

            return IssueToken(userResult.Value, Roles.Admin);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user login.");
            return Error.Unexpected();
        }
    }
    private string IssueToken(User user, Roles role) => jwtSecurityTokenHandler.WriteToken(
        new JwtSecurityToken(
            _issuer,
            _audience,
            claims:
            [
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, role.ToString()),
            ],
            signingCredentials: _signingCredentials,
            expires: DateTimeOffset.UtcNow.Add(expiration).UtcDateTime
        )
    );
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="userData">The data for the new user.</param>
    /// <returns>The newly registered User object, or an error if registration fails.</returns>
    public async Task<Result<User>> RegisterUser(User userData)
    {
         try
        {
            if (userData == null || string.IsNullOrEmpty(userData.UserName) || string.IsNullOrEmpty(userData.Email)) 
            {
                return Error.Validation("Valid user data is required for registration.");
            }

            var existingUserResult = await _userRepository.ExistsAsync(u => u.UserName == userData.UserName || u.Email == userData.Email);

            if (existingUserResult.IsError)
            {
                 return existingUserResult.Error;
            }

            if (existingUserResult.Value)
            {
                return Error.Validation("UserName or email already exists.");
            }

            userData.Password = HashPassword(userData.Password);
            userData.IsAdmin = false;

            var result = await _userRepository.AddAsync(userData);

            if (!result.IsError)
            {
                return result.Value;
            }

            return result.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user registration.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Logs in a user using a token.
    /// </summary>
    /// <param name="token">The login token.</param>
    /// <returns>The authenticated User object, or an error if login fails.</returns>
    public async Task<Result<User>> Login(string token)
    {
         try
        {
            if (string.IsNullOrEmpty(token))
            {
                return Error.Validation("Login token is required.");
            }
            Guid userIdFromToken = ValidateAndGetUserIdFromToken(token); 

            if (userIdFromToken == Guid.Empty) 
            {
                 return Error.Unauthorized("Invalid or expired token.");
            }

            var userResult = await _userRepository.GetSingleAsync<User>(u => u.UserId == userIdFromToken);

            if (userResult.IsError)
            {
                 return userResult.Error;
            }

            if (userResult?.Value == null)
            {
                return Error.NotFound("User associated with token not found.");
            }

            return userResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during token login.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Gets a user by their identifier.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>The User object, or a 404 Not Found error.</returns>
    public async Task<Result<User>> GetUser(Guid userId)
    {
         try
        {
            var result = await _userRepository.GetSingleAsync<User>(u => u.UserId == userId); // Assuming User has UserId

            if (!result.IsError)
            {
                 if (result?.Value == null)
                 {
                     return Error.NotFound($"User with ID {userId} not found.");
                 }
                return result.Value;
            }

            return result.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting a user by ID.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Allows the user to choose their language preference.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="languageCode">The code for the chosen language.</param>
    /// <returns>A Success object indicating the action was successful, or an error.</returns>
    public async Task<Result<Success>> ChooseLanguage(Guid userId, Language languageCode)
    {
         try
        {
            var updateResult = await _userRepository.UpdateRangeAsync(
                filter: u => u.UserId == userId, // Assuming User has UserId
                updateExpression: calls => calls.SetProperty(u => u.Lang, languageCode) // Assuming User has LanguageCode
            );

            if (!updateResult.IsError)
            {
                 if (updateResult.Value == 0)
                 {
                      return Error.NotFound($"User with ID {userId} not found.");
                 }
                return Success.Instance;
            }

            return updateResult.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while choosing user language.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Updates the data for an existing user.
    /// </summary>
    /// <param name="userData">The updated user data.</param>
    /// <returns>True if the update was successful, or an error.</returns>
    public async Task<Result<bool>> UpdateUserData(User userData)
    {
         try
        {
             if (userData == null)
            {
                return Error.Validation("Valid user data is required for update.");
            }

            var result = await _userRepository.UpdateAsync(userData);

            if (!result.IsError)
            {
                return true;
            }

            return result.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating user data.");
            return Error.Unexpected();
        }
    }

    /// <summary>
    /// Sets the user's notification preferences.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="allow">True to allow notifications, false to disallow.</param>
    /// <returns>A Success object indicating the action was successful, or an error.</returns>
    public async Task<Result<Success>> AllowNotifications(Guid userId, bool allow)
    {
         try
        {
            var updateResult = await _userRepository.UpdateRangeAsync(
                filter: u => u.UserId == userId, // Assuming User has UserId
                updateExpression: calls => calls.SetProperty(u => u.AllowsNotifications, allow) // Assuming User has AllowsNotifications property
            );

            if (!updateResult.IsError)
            {
                 if (updateResult.Value == 0)
                 {
                      return Error.NotFound($"User with ID {userId} not found.");
                 }
                return Success.Instance;
            }

            return updateResult.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while setting user notification preferences.");
            return Error.Unexpected();
        }
    }

    private string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password, 
            salt,
            ITERATIONS, 
            _hashAlgorithm, 
            KEY_SIZE);

        return string.Join(
            DELIMITER, 
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }

    private bool VerifyPasswordHash(string password, string storedHash)
    {
        var el = storedHash.Split(DELIMITER);
        byte[] salt = Convert.FromBase64String(el[0]);
        byte[] hash = Convert.FromBase64String(el[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            ITERATIONS, 
            _hashAlgorithm, 
            KEY_SIZE);

        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }

    private Guid ValidateAndGetUserIdFromToken(string token)
    {
        // Implement actual token validation and user ID extraction here
        // Return Guid.Empty if the token is invalid
        return Guid.Empty;
    }
}
