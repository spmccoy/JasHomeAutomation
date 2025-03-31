namespace NetDaemonApps.DomainEntities;

public abstract class MqttSensor(string groupName, string entityName, string displayName)
    : MqttEntity(HaEntityType.Sensor, groupName, entityName, displayName)
{
    private const string On = "ON";
    private const string Off = "OFF";
    
    public override void HandleStateChange(string? state)
    {
        Action action = state switch
        {
            On => HandleOn,
            Off => HandleOff,
            _ => throw new ArgumentException($"Unknown state {state}")
        };

        action();
    }

    public static string StringValueFromBool(bool value)
    {
        return value ? On : Off;
    }
    
    protected abstract void HandleOff();

    protected abstract void HandleOn();
}