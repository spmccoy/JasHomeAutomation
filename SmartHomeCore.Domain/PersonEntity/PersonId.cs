using System.Text.RegularExpressions;
using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.PersonEntity;

public class PersonId : ValueObject
{
    public const string Prefix = "person.";
    
    public string Value { get; }

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

    public static PersonId Create(string value) => new(Prefix + value);

    public static implicit operator string(PersonId personId) => personId.Value;
    
    public static explicit operator PersonId(string value) => Create(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value;
}
