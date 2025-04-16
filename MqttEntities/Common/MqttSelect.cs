

namespace Domain.Entities;

/// <summary>
/// Represents an MQTT-based select component in a Home Automation context.
/// This class provides functionality to manage selectable options for an MQTT select entity
/// and associates specific actions to be executed when an option is selected.
/// </summary>
public class MqttSelect(string groupName, string entityName, string displayName)
    : MqttEntity(HaEntityType.Select, groupName, entityName, displayName)
{
    public readonly List<string> Options = new();

    protected void AddOption(RoomState roomState)
    {
        Options.Add(roomState.ToString());
    }
}