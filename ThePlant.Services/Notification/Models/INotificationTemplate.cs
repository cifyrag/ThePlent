using System.ComponentModel.DataAnnotations;
using MediatR;

namespace ThePlant.Services.Notification.Models;

public interface INotificationTemplate : INotification
{
    [EmailAddress]
    string SendToEmail { get; set; }
    string Subject { get; set; }
    string SendToName { get; set; }
    List<string> FcmToken { get; set; } 
    Guid SendToId { get; set; }
}