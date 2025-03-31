using System.Collections.Generic;

namespace NetDaemonApps.DomainEntities;

/// <summary>
/// Represents an MQTT-based select component in a Home Automation context.
/// This class provides functionality to manage selectable options for an MQTT select entity
/// and associates specific actions to be executed when an option is selected.
/// </summary>
public class MqttSelect(string groupName, string entityName, string displayName)
    : MqttEntity(HaEntityType.Select, groupName, entityName, displayName)
{
    public readonly Dictionary<string, Action> Options = new();

    public override void HandleStateChange(string? state)
    {
        if (state != null && Options.TryGetValue(state, out var handler))
        {
            handler();
        }
        else
        {
            throw new InvalidOperationException($"Could not find an action for state {state}");
        }
    }
    
    protected void AddOption(string optionName, Action stateHandler)
    {
        ArgumentNullException.ThrowIfNull(stateHandler);
        
        if (string.IsNullOrWhiteSpace(optionName))
        {
            throw new ArgumentNullException(optionName);
        }
        
        Options.Add(optionName, stateHandler);
    }
}