namespace NetDaemonApps.DomainEntities;

/// <summary>
/// Represents the state of the Sun in Home Assistant.
/// </summary>
/// <remarks>
/// Provides predefined states for whether the Sun is above or below the horizon.
/// This class uses a private constructor to ensure that the states are immutable and predefined.
/// </remarks>
public class HaSun
{
    public static readonly HaSun AboveHorizon = new("above_horizon");
    public static readonly HaSun BelowHorizon = new("below_horizon");

    public string Value { get; }

    private HaSun(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
}