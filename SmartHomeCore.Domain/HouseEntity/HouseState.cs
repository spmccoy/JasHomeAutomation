using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.HouseEntity;

public class HouseState : ValueObject
{
    public static readonly HouseState Unknown = new("Unknown");
    public static readonly HouseState Secure = new("Secure");
    public static readonly HouseState Unsecure = new("Unsecure");
    public static readonly HouseState EveryoneAway = new("EveryoneAway");
    public static readonly HouseState Sleep = new("Sleep");

    public bool IsSecure() => this == Secure;
    public bool IsUnsecure() => this == Unsecure;
    public bool IsEveryoneAway() => this == EveryoneAway;
    public bool IsSleep() => this == Sleep;
    
    public string Value { get; }

    private HouseState(string value) => Value = value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}