using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.DoorEntity;

public class LockState : ValueObject
{
    public static readonly LockState Unknown = new("Unknown");
    public static readonly LockState Unsupported = new("Unsupported");
    public static readonly LockState Locked = new("Locked");
    public static readonly LockState Unlocked = new("Unlocked");

    public bool IsUnknown() => this == Unknown;
    public bool IsUnsupported() => this == Unsupported;
    public bool IsLocked() => this == Locked;
    public bool IsUnlocked() => this == Unlocked;
    
    public string Value { get; }

    private LockState(string value) => Value = value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}