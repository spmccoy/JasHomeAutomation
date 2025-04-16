using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

/// <summary>
/// The NotificationService class provides functionality to send notifications to specified devices.
/// Implements the <see cref="INotificationService"/> interface for managing notifications.
/// </summary>
public class NotificationService(IHaContext ha, IPersonService personService, ILogger<NotificationService> logger) : INotificationService
{
    private const string HaDomain = "notify";
    private const string AlexaService = "alexa_media";
    
    /// <inheritdoc/>
    public NotifiableDevice[] AllDevices =>
    [
        NotifiableDevice.ShawnOfficeAlexa,
        NotifiableDevice.ShawnPhone,
        NotifiableDevice.GarageAlexa
    ];
    
    /// <inheritdoc/>
    public void Notify(NotifiableDevice[] notifiableDevices, string? text = null, string? tts = null, string? title = null)
    {
        foreach (var notifiableDevice in notifiableDevices)
        {
            Notify(notifiableDevice, text, tts, title);
        }
    }

    public void Notify(NotifiableDevice notifiableDevice, string? text = null, string? tts = null, string? title = null)
    {
        var isTextToSpeech = !string.IsNullOrWhiteSpace(tts);
        var isText = !string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(text);

        if (isTextToSpeech && notifiableDevice.NotificationType == NotifiableDevice.NotificationTypeAlexa)
        {
            NotifyAlexaDevice(notifiableDevice, tts);
        }
        
        if (isText && notifiableDevice.NotificationType == NotifiableDevice.NotificationTypeHomeAssistant)
        {
            NotifyHomeAssistantDevice(notifiableDevice, text, title);
        }
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
        if (ShouldSkipNotification(notifiableDevice, out var reason))
        {
            logger.LogDebug($"Skipping notification to {notifiableDevice.Id} because {reason}.");
            return;
        }

        var notificationData = new
        {
            target = notifiableDevice.Id,
            data = new { type = "tts" },
            message
        };

        ha.CallService(HaDomain, AlexaService, data: notificationData);
    }

    private bool ShouldSkipNotification(NotifiableDevice notifiableDevice, out string reason)
    {
        if (notifiableDevice == NotifiableDevice.ShawnOfficeAlexa && personService.DontDisturbShawn)
        {
            reason = "Shawn's Do Not Disturb mode is enabled.";
            return true;
        }

        reason = string.Empty;
        return false;
    }
}