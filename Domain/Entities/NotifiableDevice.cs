namespace Domain.Entities;

/// <summary>
/// Represents a device capable of receiving notifications, with specific types of notifications based on the device.
/// </summary>
public class NotifiableDevice
{
    public const string NotificationTypeAlexa = "Alexa";
    
    public const string NotificationTypeHomeAssistant = "HomeAssistant";
    
    private NotifiableDevice(string id, string notificationType)
    {
        Id = id;
        NotificationType = notificationType;
    }
    
    public string Id { get; private init; }
    
    public string NotificationType { get; private set; }

    public static NotifiableDevice ShawnOfficeAlexa => CreateDevice("media_player.alexa_media_front_office_dot", NotificationTypeAlexa);
    
    public static NotifiableDevice ShawnPhone => CreateDevice("mobile_app_shawns_iphone", NotificationTypeHomeAssistant);
    
    public static NotifiableDevice GarageAlexa => CreateDevice("media_player.garage_echo", NotificationTypeHomeAssistant);
    
    
    private static NotifiableDevice CreateDevice(string id, string notificationType) => new NotifiableDevice(id, notificationType);
}