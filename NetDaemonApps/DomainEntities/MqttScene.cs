namespace NetDaemonApps.DomainEntities;

/// <summary>
/// Represents an MQTT-based scene with a unique identifier and display name.
/// </summary>
/// <param name="groupName">The name of the group that the scene belongs to.</param>
/// <param name="entityName">The name of the specific scene entity.</param>
/// <param name="displayName">The human-readable name used as the display name of the scene.</param>
public class MqttScene(string groupName, string entityName, string displayName)
{
    public readonly string DisplayName = displayName;
    public readonly string Id = HaMqtt.GenerateId(HaEntityType.Scene, groupName, entityName);
}