using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThePlant.API.Services.Interfaces;
using ThePlant.EF.Models;
using ThePlant.EF.Models.Enam;
using ThePlant.EF.Utils;

namespace ThePlant.API.Controllers
{
    /// <summary>
    /// API controller for managing user accounts and profiles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] 
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor for the UserController.
        /// </summary>
        /// <param name="userService">The injected IUserService.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Logs in a user with username and password.
        /// </summary>
        /// <param name="loginRequest">Object containing username and password (from body).</param>
        /// <returns>An ActionResult containing the authenticated User object or an error.</returns>
        [HttpPost("login-admin")] // POST api/User/login
        public async Task<ActionResult<User>> LoginAdmin([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and password are required for login.");
            }

            var result = await _userService.LoginAdmin(loginRequest.Username, loginRequest.Password);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }
        
        /// <summary>
        /// Logs in a user with username and password.
        /// </summary>
        /// <param name="loginRequest">Object containing username and password (from body).</param>
        /// <returns>An ActionResult containing the authenticated User object or an error.</returns>
        [HttpPost("login")] // POST api/User/login
        public async Task<ActionResult<User>> LoginUser([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and password are required for login.");
            }

            var result = await _userService.LoginUser(loginRequest.Username, loginRequest.Password);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userData">The data for the new user (from body).</param>
        /// <returns>An ActionResult containing the newly registered User object or an error.</returns>
        [HttpPost("register")] // POST api/User/register
        public async Task<ActionResult<User>> RegisterUser([FromBody] User userData)
        {
            if (userData == null || string.IsNullOrEmpty(userData.Email)) 
            {
                return BadRequest("Valid user data is required for registration.");
            }

            var result = await _userService.RegisterUser(userData);

            
            if (!result.IsError)
            {
                 return CreatedAtAction(nameof(GetUser), new { userId = result.Value.UserId }, result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Logs in a user using a token.
        /// </summary>
        /// <param name="tokenRequest">Object containing the login token (from body).</param>
        /// <returns>An ActionResult containing the authenticated User object or an error.</returns>
        [HttpPost("login/token")] // POST api/User/login/token
        public async Task<ActionResult<User>> Login([FromBody] TokenLoginRequest tokenRequest)
        {
            if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.Token))
            {
                return BadRequest("Login token is required.");
            }

            var result = await _userService.Login(tokenRequest.Token);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Gets a user by their identifier.
        /// </summary>
        /// <param name="userId">The identifier of the user (from route).</param>
        /// <returns>An ActionResult containing the User object or a 404 Not Found.</returns>
        [HttpGet("{userId}")] // GET api/User/{userId}
        [Authorize]
        public async Task<ActionResult<User>> GetUser(Guid userId)
        {
            if (userId.Equals(Guid.Empty))
            {
                return BadRequest("Valid user ID is required.");
            }

            var result = await _userService.GetUser(userId);

            
            if (!result.IsError)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Allows the user to choose their language preference.
        /// </summary>
        /// <param name="userId">The identifier of the user (from route).</param>
        /// <param name="languageRequest">Object containing the language code (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost("{userId}/language")] // POST api/User/{userId}/language
        public async Task<ActionResult<Success>> ChooseLanguage(Guid userId, [FromBody] ChooseLanguageRequest languageRequest)
        {
            if ( languageRequest == null )
            {
                return BadRequest("Valid user ID and language code are required.");
            }

            var result = await _userService.ChooseLanguage(userId, languageRequest.LanguageCode);

            
            if (!result.IsError)
            {
                return Ok(result.Value); 
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Updates the data for an existing user.
        /// </summary>
        /// <param name="userData">The updated user data (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPut] // PUT api/User
        [Authorize]
        public async Task<ActionResult> UpdateUserData([FromBody] User userData)
        {
            if (userData == null )
            {
                return BadRequest("Valid user data with an ID is required for update.");
            }

            var result = await _userService.UpdateUserData(userData);

            
            if (!result.IsError)
            {
                return NoContent();
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Sets the user's notification preferences.
        /// </summary>
        /// <param name="userId">The identifier of the user (from route).</param>
        /// <param name="notificationRequest">Object containing the allow flag (from body).</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost("{userId}/notifications")] // POST api/User/{userId}/notifications
        [Authorize]
        public async Task<ActionResult<Success>> AllowNotifications(Guid userId, [FromBody] AllowNotificationsRequest notificationRequest)
        {
            if ( notificationRequest == null)
            {
                return BadRequest("Valid user ID and notification preference are required.");
            }

            var result = await _userService.AllowNotifications(userId, notificationRequest.Allow);

            
            if (!result.IsError)
            {
                return Ok(result.Value); 
            }
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Represents the request body for the LoginUser action.
        /// </summary>
        public class LoginRequest
        {
            /// <summary>
            /// The user's username.
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// The user's password.
            /// </summary>
            public string Password { get; set; }
        }

         /// <summary>
        /// Represents the request body for the Login action using a token.
        /// </summary>
        public class TokenLoginRequest
        {
            /// <summary>
            /// The login token.
            /// </summary>
            public string Token { get; set; }
        }

        /// <summary>
        /// Represents the request body for the ChooseLanguage action.
        /// </summary>
        public class ChooseLanguageRequest
        {
            /// <summary>
            /// The code for the chosen language.
            /// </summary>
            public Language LanguageCode { get; set; }
        }

        /// <summary>
        /// Represents the request body for the AllowNotifications action.
        /// </summary>
        public class AllowNotificationsRequest
        {
            /// <summary>
            /// True to allow notifications, false to disallow.
            /// </summary>
            public bool Allow { get; set; }
        }
    }
}
