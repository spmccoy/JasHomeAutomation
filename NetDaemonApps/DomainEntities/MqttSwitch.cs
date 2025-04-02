using System.Security.Cryptography;

namespace NetDaemonApps.DomainEntities;

/// <summary>
/// Represents an abstract MQTT switch that can handle state changes and provides
/// specific behavior for "on" and "off" states through overridden methods.
/// </summary>
public abstract class MqttSwitch(string groupName, string entityName, string displayName)
    : MqttEntity(HaEntityType.Switch, groupName, entityName, displayName)
{
    public const string On = "ON";
    public const string Off = "OFF";
    public const string Unknown = "unknown";

    public bool PersistState { get; set; } = true;
    
    public override void HandleStateChange(string? state)
    {
        Action action = state switch
        {
            On => HandleOn,
            Off => HandleOff,
            Unknown => () => { },
        _ => throw new ArgumentException($"Unknown state {state}")
        };

        action();
    }

    protected abstract void HandleOff();

    protected abstract void HandleOn();
}