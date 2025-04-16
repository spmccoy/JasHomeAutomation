

namespace Domain.Entities;

public abstract class MqttEntity(HaEntityType entityType, string groupName, string entityName, string displayName)
{
    private const string Suffix = "netDaemon";
    
    public readonly string DisplayName = displayName;
    public readonly string Id = GenerateId(entityType, groupName, entityName);

    public string? InitialValue { get; protected init; }
    
    private static string GenerateId(HaEntityType entityType, string groupName, string entityName)
    {
        return $"{entityType}.{groupName}-{entityName}-{Suffix}";
    }
}