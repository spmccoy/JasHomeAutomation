namespace NetDaemonApps.DomainEntities;

public abstract class MqttEntity(HaEntityType entityType, string groupName, string entityName, string displayName)
{
    public readonly string DisplayName = displayName;
    public readonly string Id = HaMqtt.GenerateId(entityType, groupName, entityName);

    public abstract void HandleStateChange(string? state);
}