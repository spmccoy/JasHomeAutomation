using MqttEntities.Common;

namespace MqttEntities.MainRoom;

/// <summary>
/// Main switch for shawn's room that can determine what state the room should be in.
/// </summary>
public class MainRoomStateSwitch : MqttSwitch
{
    /// <summary>
    /// Main switch for shawn's room that can determine what state the room should be in.
    /// </summary>
    public MainRoomStateSwitch() 
        : base("MainRoom", "state-switch", "Main Room State Switch")
    {
    }
}