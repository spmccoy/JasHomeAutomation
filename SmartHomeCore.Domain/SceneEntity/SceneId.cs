using System.Text.RegularExpressions;
using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.SceneEntity;

public class SceneId : ValueObject
{
    private const string Prefix = "scene.";
    
    private SceneId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Scene ID cannot be empty", nameof(value));
        }
        
        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9_\.]+$"))
        {
            throw new ArgumentException("Scene ID can only contain letters, numbers, periods, and underscores", nameof(value));
        }
            
        Value = value;
    }
    
    public string Value { get; }

    public static SceneId From(string value) => new(Prefix + value);

    public static implicit operator string(SceneId sceneId) => sceneId.Value;
    
    public static explicit operator SceneId(string value) => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value;
}