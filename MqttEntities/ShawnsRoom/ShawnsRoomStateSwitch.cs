using Domain.Entities;

namespace MqttEntities.ShawnsRoom;

/// <summary>
/// Main switch for shawn's room that can determine what state the room should be in.
/// </summary>
public class ShawnsRoomStateSwitch : MqttSwitch
{
    /// <summary>
    /// Main switch for shawn's room that can determine what state the room should be in.
    /// </summary>
    public ShawnsRoomStateSwitch() 
        : base("ShawnRoom", "state", "Shawn's Room State Switch")
    {
    }
}