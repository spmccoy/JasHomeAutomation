using NetDaemonApps.Services;

namespace NetDaemonApps.apps.ShawnRoom.Controls;

/// <summary>
/// Main switch for shawn's room that can determine what state the room should be in.
/// </summary>
public class ShawnsRoomStateSwitch : MqttSwitch
{
    private readonly Entities _entities;
    private readonly IShawnRoomService _shawnRoomService;

    /// <summary>
    /// Main switch for shawn's room that can determine what state the room should be in.
    /// </summary>
    public ShawnsRoomStateSwitch(Entities entities, IShawnRoomService shawnRoomService) 
        : base("ShawnRoom", "state", "Shawn's Room State Switch")
    {
        _entities = entities;
        _shawnRoomService = shawnRoomService;

        PersistState = false;
    }

    protected override void HandleOff()
    {
        _entities.Select.ShawnroomStateNetdaemon.SelectOption(RoomState.Off);
    }

    protected override void HandleOn()
    {
        _shawnRoomService.DetermineAndSetRoomState();
    }
}