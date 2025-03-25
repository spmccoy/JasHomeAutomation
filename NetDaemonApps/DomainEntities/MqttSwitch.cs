using HomeAssistantGenerated;

namespace NetDaemonApps.DomainEntities;

/// <summary>
/// Represents an abstract MQTT switch that can handle state changes and provides
/// specific behavior for "on" and "off" states through overridden methods.
/// </summary>
public abstract class MqttSwitch(Entities entities, string groupName, string entityName, string displayName)
{
    public readonly string DisplayName = displayName;
    public readonly string Id = HaMqtt.GenerateId(HaEntityType.Switch, groupName, entityName);
    private const string On = "ON";
    private const string Off = "OFF";
    
    public void HandleStateChange(string state)
    {
        Action action = state switch
        {
            On => HandleOn,
            Off => HandleOff,
            _ => throw new ArgumentException($"Unknown state {state}")
        };

        action();
    }

    public abstract void HandleOff();

    public abstract void HandleOn();
}