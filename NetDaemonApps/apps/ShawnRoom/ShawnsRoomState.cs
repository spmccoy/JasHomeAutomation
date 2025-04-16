

using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.ShawnRoom.Devices;

[NetDaemonApp]
public class ShawnsRoomState
{
    private readonly IShawnRoomService _shawnRoomService;
    
    public ShawnsRoomState(IShawnRoomService shawnRoomService)
    {
        _shawnRoomService = shawnRoomService;
        
        shawnRoomService.Select.StateChanges()
            .SubscribeAsyncConcurrent(ProcessSelectChangeAsync);

        shawnRoomService.Switch.StateChanges()
            .Subscribe(_ => shawnRoomService.HandleSwitch());
    }

    private async Task ProcessSelectChangeAsync(StateChange<SelectEntity, EntityState<SelectAttributes>> stateChange)
    {
        if (!_shawnRoomService.AreSwitchAndSelectStatesInSync())
        {
            _shawnRoomService.SyncSwitchToSelect();
        }

        var roomState = RoomStates.FromString(stateChange.New?.State);
        
        switch (roomState.Value)
        {
            case RoomStateValue.Off:
                await _shawnRoomService.HandleOffAsync();
                break;

            case RoomStateValue.Day:
                _shawnRoomService.HandleDay();
                break;

            case RoomStateValue.Night:
                _shawnRoomService.HandleNight();
                break;

            case RoomStateValue.Gaming:
                _shawnRoomService.HandleGaming();
                break;

            case RoomStateValue.SimRacing:
                _shawnRoomService.HandleSimRacing();
                break;

            default:
                throw new ArgumentOutOfRangeException(
                    stateChange.New?.State, 
                    $"Unexpected RoomState: {stateChange.New?.State}"
                );
        }
    }
}