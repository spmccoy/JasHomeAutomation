using System.Text.RegularExpressions;
using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.DoorEntity;

public class DoorId : ValueObject
{
    public string Value { get; }

    private DoorId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Door ID cannot be empty", nameof(value));
        }
        
        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9_]+$"))
        {
            throw new ArgumentException("Door ID can only contain letters, numbers, and underscores", nameof(value));
        }
            
        Value = value;
    }

    public static DoorId Create(string value) => new(value);

    public static implicit operator string(DoorId personId) => personId.Value;
    
    public static explicit operator DoorId(string value) => Create(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value;
}