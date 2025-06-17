namespace SmartHomeCore.Application.Common;

public interface INotificationClient
{
    void Notify(string message);
}