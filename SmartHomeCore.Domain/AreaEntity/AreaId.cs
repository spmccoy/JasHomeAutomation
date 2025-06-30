using System.Text.RegularExpressions;
using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.AreaEntity;

public class AreaId : ValueObject
{
    public string Value { get; }

    private AreaId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Area ID cannot be empty", nameof(value));
        }
        
        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9_]+$"))
        {
            throw new ArgumentException("Area ID can only contain letters, numbers, and underscores", nameof(value));
        }
            
        Value = value;
    }

    public static AreaId From(string value) => new(value);

    public static implicit operator string(AreaId areaId) => areaId.Value;

    public static explicit operator AreaId(string value) => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value;
}