using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.HouseEntity;

public class HouseState : ValueObject
{
    private readonly string _value;
    
    public static readonly HouseState Unknown = new("Unknown");
    public static readonly HouseState Secure = new("Secure");
    public static readonly HouseState Unsecure = new("Unsecure");
    public static readonly HouseState EveryoneAway = new("EveryoneAway");
    public static readonly HouseState Sleep = new("Sleep");

    public static readonly HouseState[] AllStates =
    [
        Unknown,
        Secure,
        Unsecure,
        EveryoneAway,
        Sleep
    ];

    private HouseState(string value)
    {
        _value = value;
    }

    public static HouseState? From(string? value)
    {
        return string.IsNullOrWhiteSpace(value) 
            ? null 
            : AllStates.FirstOrDefault(s => (string)s == value);
    }

    
    public bool IsSecure() => this == Secure;
    public bool IsUnsecure() => this == Unsecure;
    public bool IsEveryoneAway() => this == EveryoneAway;
    public bool IsSleep() => this == Sleep;
    
    public static implicit operator string(HouseState houseState) => houseState._value;
    public static explicit operator HouseState(string value) => new (value);

    public override string ToString() => _value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _value;
    }
}