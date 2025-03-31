namespace NetDaemonApps.DomainEntities;

public abstract class MqttEntity(HaEntityType entityType, string groupName, string entityName, string displayName)
{
    private const string MqttPrefix = "netDaemon";
    public readonly string DisplayName = displayName;
    public readonly string Id = GenerateId(entityType, groupName, entityName);

    private static string GenerateId(HaEntityType entityType, string groupName, string entityName)
    {
        return $"{entityType}.{MqttPrefix}_{groupName}_{entityName}";
    }
    
    public abstract void HandleStateChange(string? state);
}