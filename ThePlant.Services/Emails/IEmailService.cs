using PostmarkDotNet;

namespace ThePlant.Services.Emails;

public interface IEmailService
{
    Task<PostmarkResponse?> SendEmailAsync(string toEmail, string? subject, string htmlTemplate,
        Dictionary<string, string> templateData);
}