

using Domain.Entities;

namespace MqttEntities.Common;

public abstract class MqttSensor(string groupName, string entityName, string displayName)
    : MqttEntity(HaEntityType.Sensor, groupName, entityName, displayName)
{
    private const string On = "ON";
    private const string Off = "OFF";
    
    public static string StringValueFromBool(bool value)
    {
        return value ? On : Off;
    }
    
    protected abstract void HandleOff();

    protected abstract void HandleOn();
}