using NetDaemonApps.Interfaces;
using NetDaemonApps.Services;

namespace NetDaemonApps.apps.MainRoom.Controls;

/// <summary>
/// Main switch for shawn's room that can determine what state the room should be in.
/// </summary>
public class MainRoomStateSwitch : MqttSwitch
{
    private readonly IMainRoomService _mainRoomService;
    private readonly SelectEntity _mainRoomSelect;

    /// <summary>
    /// Main switch for shawn's room that can determine what state the room should be in.
    /// </summary>
    public MainRoomStateSwitch(
        Entities entities, 
        IMainRoomService mainRoomService) 
        : base("MainRoom", "state-switch", "Main Room State Switch")
    {
        _mainRoomService = mainRoomService;
        _mainRoomSelect = entities.Select.MainroomStateSelectNetdaemon;

        PersistState = false;
    }

    protected override void HandleOff()
    {
        _mainRoomSelect.SelectOption(RoomState.Off);   
    }

    protected override void HandleOn()
    {
        _mainRoomService.DetermineAndSetRoomState();
    }
}