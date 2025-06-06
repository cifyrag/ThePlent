using Microsoft.AspNetCore.Http;
using System;

namespace UI.Services
{
    public static class TokenService
    {
        public static void PostToken(string token, HttpContext httpContext)
        {
            // Assuming 'token' is the JWT string
            httpContext.Response.Cookies.Append("Token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true if using HTTPS
                Expires = DateTime.UtcNow.AddHours(1) // Adjust expiration as needed
                // SameSite = SameSiteMode.Strict // Consider SameSite attribute for security
            });
        }
    }
}