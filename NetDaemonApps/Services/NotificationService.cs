using System.Linq;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

/// <summary>
/// The NotificationService class provides functionality to send notifications to specified devices.
/// Implements the <see cref="INotificationService"/> interface for managing notifications.
/// </summary>
public class NotificationService(IHaContext ha) : INotificationService
{
    private const string HaDomain = "notify";
    
    /// <inheritdoc/>
    public NotifiableDevice[] AllDevices =>
    [
        NotifiableDevice.ShawnOffice(),
        NotifiableDevice.ShawnPhone()
    ];

    
    /// <inheritdoc/>
    public void Notify(NotifiableDevice[] notifiableDevices, string? text = null, string? tts = null, string? title = null)
    {
        notifiableDevices.Where(w => w.NotificationType == NotifiableDevice.AlexaNotification)
            .ToList()
            .ForEach(notifiableDevice => NotifyAlexaDevice(notifiableDevice, tts));
        
        notifiableDevices.Where(w => w.NotificationType == NotifiableDevice.HomeAssistantNotification)
            .ToList()
            .ForEach(notifiableDevice => NotifyHomeAssistantDevice(notifiableDevice, text, title));
    }

    /// <summary>
    /// Sends a notification to a specific device using Home Assistant service.
    /// </summary>
    /// <param name="notifiableDevice">The device to which the notification is sent.</param>
    /// <param name="message">The message content of the notification. Optional.</param>
    /// <param name="title">The title of the notification. Optional.</param>
    private void NotifyHomeAssistantDevice(NotifiableDevice notifiableDevice, string? message, string? title)
    {
        var data = new
        {
            message,
            title
        };
        
        ha.CallService(HaDomain, notifiableDevice.Id, data: data);
    }

    /// <summary>
    /// Sends a notification specifically to an Alexa device using Text-To-Speech (TTS).
    /// </summary>
    /// <param name="notifiableDevice">The Alexa device to which the notification will be sent. It must be of type <see cref="NotifiableDevice"/> and have a notification type set to AlexaNotification.</param>
    /// <param name="message">The message to send as TTS content to the specified Alexa device. Can be null or an empty string if no message is provided.</param>
    private void NotifyAlexaDevice(NotifiableDevice notifiableDevice, string? message)
    {
        const string haService = "alexa_media";
        
        var data = new
        {
            target = notifiableDevice.Id,
            data = new { type = "tts"},
            message
        };
        
        ha.CallService(HaDomain, haService, data: data);
    }
}