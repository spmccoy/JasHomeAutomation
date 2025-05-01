namespace MqttEntities.Models;

public record RoomState(RoomStateValue Value, string Name)
{
    public override string ToString() => Name;
}

public enum RoomStateValue
{
    Unknown,
    Off,
    Day,
    Night,
    Dawn,
    Dusk,
    Tv,
    Gaming,
    SimRacing,
    HomeSecure,
    HomeUnsecured,
    Away,
    Sleep
}

public static class RoomStates
{
    public static readonly RoomState Unknown = new(RoomStateValue.Unknown, "Unknown");
    public static readonly RoomState Off = new(RoomStateValue.Off, "Off");
    public static readonly RoomState Day = new(RoomStateValue.Day, "Day");
    public static readonly RoomState Night = new(RoomStateValue.Night, "Night");
    public static readonly RoomState Dawn = new(RoomStateValue.Dawn, "Dawn");
    public static readonly RoomState Dusk = new(RoomStateValue.Dusk, "Dusk");
    public static readonly RoomState Tv = new(RoomStateValue.Tv, "Tv");
    public static readonly RoomState Gaming = new(RoomStateValue.Gaming, "Gaming");
    public static readonly RoomState SimRacing = new(RoomStateValue.SimRacing, "Sim Racing");
    public static readonly RoomState HomeSecure = new(RoomStateValue.HomeSecure, "Home Secure");
    public static readonly RoomState HomeUnsecured = new(RoomStateValue.HomeUnsecured, "Home Unsecured");
    public static readonly RoomState Away = new(RoomStateValue.Away, "Away");
    public static readonly RoomState Sleep = new(RoomStateValue.Sleep, "Sleep");

    public static RoomState FromString(string? name) => string.IsNullOrWhiteSpace(name)
                                                        ? Unknown
                                                        : RoomStateMap.GetValueOrDefault(name, Unknown);
    
    private static readonly Dictionary<string, RoomState> RoomStateMap = new()
    {
        { Unknown.Name, Unknown },
        { Off.Name, Off },
        { Day.Name, Day },
        { Night.Name, Night },
        { Dawn.Name, Dawn },
        { Dusk.Name, Dusk },
        { Tv.Name, Tv },
        { Gaming.Name, Gaming },
        { SimRacing.Name, SimRacing },
        { HomeSecure.Name, HomeSecure },
        { HomeUnsecured.Name, HomeUnsecured },
        { Away.Name, Away },
        { Sleep.Name, Sleep }
    };
}