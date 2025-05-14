using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using PostmarkDotNet;
using ThePlant.EF.Settings;
using ThePlant.Services.Services;

namespace ThePlant.Services.Emails;

public class EmailPostmarkService:IEmailService
{
    private readonly string _postmarkApiKey;
    private readonly string _postmarkFrom;
    private readonly ILogger<EmailPostmarkService> _logger;

    public EmailPostmarkService(IOptions<EmailSettings> emailSetting, ILogger<EmailPostmarkService> logger)
    {
        _logger = logger;
        _postmarkApiKey = emailSetting.Value.ApiKey;
        _postmarkFrom = emailSetting.Value.From;
    }

    public async Task<PostmarkResponse?> SendEmailAsync(string toEmail, string? subject, string htmlTemplate, Dictionary<string, string> templateData)
    {
        try
        {
            var client = new PostmarkClient(_postmarkApiKey);

            var htmlContent = TemplateHelper.ReplaceTemplateData(htmlTemplate, templateData);
            var plainTextContent = StripHtmlTags(htmlContent);

            var message = new PostmarkMessage
            {
                From = _postmarkFrom,
                To = toEmail,
                Subject = subject ?? string.Empty,
                HtmlBody = htmlContent,
                TextBody = plainTextContent,
                TrackOpens = true
            };

            var response = await client.SendMessageAsync(message);

            if (response.Status == PostmarkStatus.Success)
            {
                _logger.LogInformation($"Email sent successfully to {toEmail}");
            }
            else
            {
                _logger.LogError($"Failed to send email to {toEmail}: {response.Message}");
                _logger.LogInformation("TextBody: {TextBody}", plainTextContent);
                _logger.LogInformation("HtmlBody: {HtmlBody}", htmlContent);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {ToEmail}, subject: {Subject}", toEmail, subject);
            
            return null;
        }
    }

    private string StripHtmlTags(string htmlContent)
    {
        return Regex.Replace(htmlContent, "<.*?>", string.Empty);
    }
}