namespace NetDaemonApps.DomainEntities;

/// <summary>
/// Represents a device capable of receiving notifications, with specific types of notifications based on the device.
/// </summary>
public class NotifiableDevice
{
    public const string AlexaNotification = "Alexa";
    
    public const string HomeAssistantNotification = "HomeAssistant";
    
    private NotifiableDevice(string id, string notificationType)
    {
        Id = id;
        NotificationType = notificationType;
    }
    
    public string Id { get; private init; }
    
    public string NotificationType { get; private set; }

    public static NotifiableDevice ShawnOffice()
    {
        return new NotifiableDevice("media_player.alexa_media_front_office_dot", AlexaNotification);
    }
    
    public static NotifiableDevice ShawnPhone()
    {
        return new NotifiableDevice("mobile_app_shawns_iphone", HomeAssistantNotification);
    }
}