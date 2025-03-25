using System.Collections.Generic;

namespace NetDaemonApps.DomainEntities;

/// <summary>
/// Represents an MQTT-based select component in a Home Automation context.
/// This class provides functionality to manage selectable options for an MQTT select entity
/// and associates specific actions to be executed when an option is selected.
/// </summary>
public class MqttSelect(Entities entities, string groupName, string entityName, string displayName)
{
    public readonly string DisplayName = displayName;
    public readonly string Id = HaMqtt.GenerateId(HaEntityType.Select, groupName, entityName);
    
    public readonly Dictionary<string, Action> StateHandlers = new();

    protected void AddOption(string optionName, Action stateHandler)
    {
        ArgumentNullException.ThrowIfNull(stateHandler);
        
        if (string.IsNullOrWhiteSpace(optionName))
        {
            throw new ArgumentNullException(optionName);
        }
        
        StateHandlers.Add(optionName, stateHandler);
    }
}