using MqttEntities.Models;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.MainRoom;

[NetDaemonApp]
public class MainRoomState
{
    public MainRoomState(IMainRoomService mainRoomService)
    {
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Dusk.ToString())
            .Subscribe(_ => mainRoomService.HandleDusk());
        
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Dawn.ToString())
            .Subscribe(_ => mainRoomService.HandleDawn());
        
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Off.ToString())
            .Subscribe(_ => mainRoomService.HandleOff());
        
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Day.ToString())
            .Subscribe(_ => mainRoomService.HandleDay());
        
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Night.ToString())
            .Subscribe(_ => mainRoomService.HandleNight());
        
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Tv.ToString())
            .Subscribe(_ => mainRoomService.HandleTv());
    }
}