using Domain.Entities;
using MqttEntities.Common;

namespace MqttEntities.House;

public class CameraNotificationsSwitch : MqttSwitch
{
    public CameraNotificationsSwitch() 
        : base("House", "camera-notifications", "Camera notifications")
    {
        InitialValue = On;
    }
}