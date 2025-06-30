using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.AreaEntity;

public class AreaState : ValueObject
{
    private readonly string _value;
    
    public static readonly AreaState Unknown = new("Unknown");
    public static readonly AreaState Off = new("Off");
    public static readonly AreaState Day = new("Day");
    public static readonly AreaState Night = new("Night");
    public static readonly AreaState Dusk = new("Dusk");
    public static readonly AreaState Dawn = new("Dawn");
    public static readonly AreaState Gaming = new("Gaming");

    public static readonly AreaState[] AllStates =
    [
        Unknown,
        Off,
        Day,
        Night,
        Dusk,
        Dawn,
        Gaming
    ];
    
    private AreaState(string value)
    {
        _value = value;
    }
    
    public static AreaState? From(string? value)
    {
        return string.IsNullOrWhiteSpace(value) 
            ? null 
            : AllStates.FirstOrDefault(s => (string)s == value);
    }

    public bool IsUnknown() => this == Unknown;
    public bool IsOff() => this == Off;
    public bool IsDay() => this == Day;
    public bool IsNight() => this == Night;
    public bool IsDusk() => this == Dusk;
    public bool IsDawn() => this == Dawn;
    public bool IsGaming() => this == Gaming;
    
    public static implicit operator string(AreaState areaState) => areaState._value;
    
    public override string ToString() => _value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _value;
    }
}