using NetDaemonApps.Models;

namespace NetDaemonApps.Interfaces;

/// <summary>
/// Defines a service to manage notifications for various devices.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends notifications to a list of notifiable devices.
    /// Depending on the NotificationType of the devices, the notification is sent through Alexa or Home Assistant.
    /// </summary>
    /// <param name="notifiableDevices">An array of <see cref="NotifiableDevice"/> to which the notification will be sent.</param>
    /// <param name="text">The notification text. Used for Home Assistant notifications.</param>
    /// <param name="tts">The text-to-speech message. Used for Alexa notifications.</param>
    /// <param name="title">The title of the notification. Used for Home Assistant notifications.</param>
    /// <code>
    /// var notificationService = new NotificationService(haContext);
    /// 
    /// // Notify all devices
    /// notificationService.Notify(notificationService.AllDevices, "Hello World", "This is a TTS message", "Notification Title");
    /// 
    /// // Notify specific devices
    /// var devices = new[]
    /// {
    ///     NotifiableDevice.ShawnPhone(),
    ///     NotifiableDevice.ShawnOffice()
    /// };
    /// notificationService.Notify(devices, "Device-Specific Message");
    /// </code>
    void Notify(NotifiableDevice[] notifiableDevices, string? text = null, string? tts = null, string? title = null);

    /// <summary>
    /// Gets an array of all notifiable devices available for sending notifications.
    /// This property includes predefined devices that can be used to send notifications
    /// via Alexa or Home Assistant based on their notification type.
    /// </summary>
    NotifiableDevice[] AllDevices { get; }

    void Notify(NotifiableDevice notifiableDevice, string? text = null, string? tts = null, string? title = null);
}