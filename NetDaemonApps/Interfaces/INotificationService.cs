using NetDaemonApps.Models;

namespace NetDaemonApps.Interfaces;

public interface INotificationService
{
    NotifiableDevice ShawnOfficeAlexa { get; }
    NotifiableDevice GarageAlexa { get; }
    NotifiableDevice KitchenAlexa { get; }
    NotifiableDevice ShawnPhone { get; }
    NotifiableDevice[] GetAllDevices { get; }
    void Notify(Notification notification);
}