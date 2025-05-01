using System.Collections.Generic;
using System.Linq;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.Services;

/// <summary>
/// The NotificationService class provides functionality to send notifications to specified devices.
/// Implements the <see cref="INotificationService"/> interface for managing notifications.
/// </summary>
public class NotificationService(IHaContext ha, IPersonService personService, ILogger<NotificationService> logger) : INotificationService
{
    private const string HaDomain = "notify";
    
    public NotifiableDevice ShawnOfficeAlexa { get; } = new(
            "Shawn's Office Alexa", 
            "alexa_media_front_office_dot",
            NotifiableDevice.NotificationDeviceType.Alexa);

    public NotifiableDevice GarageAlexa { get; } = new(
        "Garage Alexa",
        "alexa_media_garage_echo",
        NotifiableDevice.NotificationDeviceType.Alexa);

    public NotifiableDevice KitchenAlexa { get; } = new(
        "Kitchen Alexa",
        "alexa_media_kitchen",
        NotifiableDevice.NotificationDeviceType.Alexa);

    public NotifiableDevice ShawnPhone { get; } = new(
        "Shawn's Phone",
        "mobile_app_shawns_iphone",
        NotifiableDevice.NotificationDeviceType.HomeAssistant);
    
    public NotifiableDevice[] GetAllDevices =>
    [
        ShawnOfficeAlexa,
        KitchenAlexa,
        ShawnPhone,
        GarageAlexa,
    ];

    public void Notify(Notification notification)
    {
        foreach (var notifiableDevice in notification.Devices)
        {
            switch (notifiableDevice.DeviceType)
            {
                case NotifiableDevice.NotificationDeviceType.Alexa when notification.HasTts:
                    NotifyAlexaDevice(notifiableDevice, notification.Tts);
                    break;
                
                case NotifiableDevice.NotificationDeviceType.HomeAssistant when notification.HasText:
                    NotifyHomeAssistantDevice(notifiableDevice, notification.Text, notification.Title);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            data = new { type = "tts" },
            message
        };
        ha.CallService(HaDomain, notifiableDevice.Id, data: notificationData);
    }

    private bool ShouldSkipNotification(NotifiableDevice notifiableDevice, out string reason)
    {
        var shouldSkip = false;
        var reasons = new List<string>();
        
        if (notifiableDevice == ShawnOfficeAlexa && personService.DontDisturbShawn)
        {
            reasons.Add("Shawn's Do Not Disturb mode is enabled.");
            shouldSkip = true;
        }

        reason = string.Join(", ", reasons);
        return shouldSkip;
    }
}