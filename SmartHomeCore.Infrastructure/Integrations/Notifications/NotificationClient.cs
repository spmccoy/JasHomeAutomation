using Microsoft.Extensions.Logging;
using SmartHomeCore.Application.Common;

namespace SmartHomeCore.Infrastructure.Integrations.Notifications;

public class NotificationClient(ILogger<NotificationClient> logger) : INotificationClient
{
    public void Notify(string message)
    {
        logger.LogInformation("Sending notification message: {Message}", message);
    }
}