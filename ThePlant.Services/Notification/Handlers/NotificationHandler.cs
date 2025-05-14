using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Common;
using ThePlant.Services.Notification.Models;
using ThePlant.Services.Emails;
using ThePlant.Services.Services;

namespace ThePlant.Services.Notification.Handlers;

public class NotificationHandler<T> : INotificationHandler<T> where T : class, INotificationTemplate
{
    private readonly IEmailService _emailService;
    private readonly ILogger<NotificationHandler<T>> _logger;

    public NotificationHandler(
        IEmailService emailService,
        ILogger<NotificationHandler<T>> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        try
        {
            var templateData = await TemplateHelper.ToTemplateData(notification);
            var nameOfTemplate =notification.ToString()+"_"+typeof(T).Name;
            var emailTemplate = FilesTemplateHelper.ReadEmailTemplate(nameOfTemplate);

            if (!emailTemplate.IsNullOrEmpty() && emailTemplate != null)
            {
                await _emailService.SendEmailAsync(
                    toEmail: notification.SendToEmail,
                    subject: notification.Subject,
                    htmlTemplate: emailTemplate,
                    templateData: templateData
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send notification. Error: {ex.Message}. Notification: {typeof(T).Name}. FCM Token: {notification.FcmToken}");
        }
    }
}
