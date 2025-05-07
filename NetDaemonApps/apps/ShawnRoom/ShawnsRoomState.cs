using System.Threading.Tasks;
using MqttEntities.Models;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Extensions;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class ShawnsRoomState
{
    private readonly IShawnRoomService _shawnRoomService;
    private readonly SwitchEntities _switches;

    public ShawnsRoomState(
        IShawnRoomService shawnRoomService, 
        SelectEntities selects,
        FanEntities fans,
        ILightService lightService,
        RemoteEntities remotes,
        SceneEntities scenes,
        SwitchEntities switches)
    {
        _shawnRoomService = shawnRoomService;
        _switches = switches;

        selects.ShawnroomStateNetdaemon.StateChanges()
            .Subscribe(stateChange =>
            {
                var newRoomState = RoomStates.FromString(stateChange.New?.State);

                switch (newRoomState.Value)
                {
                    case RoomStateValue.Off:
                        fans.ShawnPurifier.SetPercentage(100);
                        lightService.TurnOffAreaLights(HaArea.ShawnRoom);
                        remotes.ShawnSOfficeTv.TurnOff();
                        EnsureSwitchOff();
                        break;

                    case RoomStateValue.Day:
                        fans.ShawnPurifier.SetPercentage(33);
                        scenes.ShawnSOfficeDay.TurnOn();
                        EnsureSwitchOn();
                        break;

                    case RoomStateValue.Night:
                        fans.ShawnPurifier.SetPercentage(33);
                        scenes.ShawnsOfficeNight.TurnOn();
                        EnsureSwitchOn();
                        break;

                    case RoomStateValue.Gaming:
                        EnsureSwitchOn();
                        break;

                    case RoomStateValue.SimRacing:
                        EnsureSwitchOn();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(
                            stateChange.New?.State, 
                            $"Unexpected RoomState: {stateChange.New?.State}"
                        );
                }
            });

        switches.ShawnroomStateNetdaemon.StateChanges()
            .Subscribe(stateChange =>
            {
                var newState = stateChange.New?.State;
                var oldState = stateChange.Old?.State;
                
                switch (newState)
                {
                    case HaState.On when oldState == HaState.Off:
                        shawnRoomService.DetermineAndSetRoomState();
                        break;

                    case HaState.Off:
                        selects.ShawnroomStateNetdaemon.SelectOff();
                        break;
                }
            });
    }

    private void EnsureSwitchOff()
    {
        if (_switches.ShawnroomStateNetdaemon.State != HaState.Off)
        {
            _switches.ShawnroomStateNetdaemon.TurnOff();
        }
    }
    
    private void EnsureSwitchOn()
    {
        if (_switches.ShawnroomStateNetdaemon.State != HaState.On)
        {
            _switches.ShawnroomStateNetdaemon.TurnOn();
        }
    }
}