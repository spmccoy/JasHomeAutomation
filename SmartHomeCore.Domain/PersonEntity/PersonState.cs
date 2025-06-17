using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.PersonEntity;

public class PersonState : ValueObject
{
    public static readonly PersonState Unknown = new("Unknown"); 
    public static readonly PersonState Home = new("Home"); 
    public static readonly PersonState Away = new("Away");
    
    public bool IsHome() => this == Home;
    public bool IsAway() => this == Away;
    
    public string Value { get; }

    private PersonState(string value) => Value = value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}