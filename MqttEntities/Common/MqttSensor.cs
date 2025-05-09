using MqttEntities.Models;

namespace MqttEntities.Common;

public abstract class MqttSensor(string groupName, string entityName, string displayName, string deviceClass)
    : MqttEntity(HaEntityType.Sensor, groupName, entityName, displayName)
{
    public string DeviceClass { get; private set; } = deviceClass;
}