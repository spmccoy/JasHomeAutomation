using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.DoorEntity;

public class DoorState : ValueObject
{
    public static readonly DoorState Unknown = new("Unknown");
    public static readonly DoorState Unsupported = new("Unsupported");
    public static readonly DoorState Open = new("Open");
    public static readonly DoorState Closed = new("Closed");
    
    public bool IsUnknown() => this == Unknown;
    public bool IsUnsupported() => this == Unsupported;
    public bool IsOpen() => this == Open;
    public bool IsClosed() => this == Closed;
    
    public string Value { get; }

    private DoorState(string value) => Value = value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}