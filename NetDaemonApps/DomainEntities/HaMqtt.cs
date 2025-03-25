namespace NetDaemonApps.DomainEntities;

/// <summary>
/// Provides utilities for generating unique MQTT entity identifiers within the NetDaemonApps
/// domain entities framework for Home Assistant.
/// </summary>
/// <remarks>
/// This class is used for creating standardized and unique MQTT entity IDs based on a specific
/// prefix, entity type, group name, and entity name. It helps to ensure consistency across
/// different components that interact with MQTT-based entities in Home Assistant.
/// </remarks>
public static class HaMqtt
{
    private const string MqttPrefix = "netDaemon";

    public static string GenerateId(HaEntityType entityType, string groupName, string entityName)
    {
        return $"{entityType}.{MqttPrefix}_{groupName}_{entityName}";
    }
}