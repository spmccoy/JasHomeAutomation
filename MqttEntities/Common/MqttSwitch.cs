

namespace Domain.Entities;

/// <summary>
/// Represents an abstract MQTT switch that can handle state changes and provides
/// specific behavior for "on" and "off" states through overridden methods.
/// </summary>
public abstract class MqttSwitch(string groupName, string entityName, string displayName)
    : MqttEntity(HaEntityType.Switch, groupName, entityName, displayName)
{
    protected const string On = "ON";
    protected const string Off = "OFF";
}