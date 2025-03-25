namespace NetDaemonApps.DomainEntities;

/// <summary>
/// Represents a specific type of HA (Home Assistant) entity that defines the behavior
/// and characteristics of a particular home automation component within the system.
/// </summary>
/// <remarks>
/// This class is typically used to categorize and manage various entity types
/// in a home automation context. It aids in organizing and identifying
/// entities based on their function or role, such as lights, sensors, or switches.
/// </remarks>
public class HaEntityType
{
    public static readonly HaEntityType Switch = new("switch");
    public static readonly HaEntityType Select = new("select");
    public static readonly HaEntityType Scene = new("scene");

    public string Value { get; }

    private HaEntityType(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
}