using System.Text.RegularExpressions;
using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.PersonEntity;

public class PersonId : ValueObject
{
    private const string Prefix = "person.";
    
    /// <summary>
    /// Represents a value object for a person's unique identifier.
    /// </summary>
    private PersonId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Person ID cannot be empty", nameof(value));
        }
        
        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9_\.]+$"))
        {
            throw new ArgumentException("Person ID can only contain letters, numbers, and underscores", nameof(value));
        }
            
        Value = value;
    }
    
    public string Value { get; }

    public static PersonId From(string value) => new(Prefix + value);

    public static implicit operator string(PersonId personId) => personId.Value;
    
    public static explicit operator PersonId(string value) => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value;
}
