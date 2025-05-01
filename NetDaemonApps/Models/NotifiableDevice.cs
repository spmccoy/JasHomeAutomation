namespace NetDaemonApps.Models;

/// <summary>
/// Represents a device capable of receiving notifications, with specific types of notifications based on the device.
/// </summary>
public class NotifiableDevice
{
    public NotifiableDevice(string name, string id, NotificationDeviceType notificationDeviceType)
    {
        Id = id;
        Name = name;
    }
    
    public string Id { get; init; }
    
    public string Name { get; init; }
    
    public NotificationDeviceType DeviceType { get; init; }
    
    public enum NotificationDeviceType
    {
        Alexa,
        HomeAssistant
    }
}