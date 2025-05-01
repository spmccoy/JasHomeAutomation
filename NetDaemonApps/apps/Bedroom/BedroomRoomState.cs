using MqttEntities.Models;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.Bedroom;

[NetDaemonApp]
public class BedroomRoomState
{
    public BedroomRoomState(SelectEntities selects, FanEntities fans, LightEntities lights, ScheduleEntities calendars, IPersonService personService)
    {
        selects.BedroomStateNetdaemon.StateChanges()
            .Subscribe(stateChange =>
            {
                var newState = RoomStates.FromString(stateChange.New?.State).Value;

                switch (newState)
                {
                    case RoomStateValue.Day:
                        fans.Bedroom.SetPercentage(33);
                        lights.BedroomDownLight.TurnOff();
                        lights.BedLeds.TurnOff();
                        lights.NightstandLights.TurnOff();
                        break;
                    
                    case RoomStateValue.Off:
                        fans.Bedroom.TurnOff();
                        lights.BedroomDownLight.TurnOff();
                        lights.BedLeds.TurnOff();
                        lights.NightstandLights.TurnOff();
                        break;
                    
                    case RoomStateValue.Night:
                        fans.Bedroom.SetPercentage(33);
                        lights.BedroomDownLight.TurnOff();
                        lights.BedLeds.TurnOn();
                        lights.NightstandLights.TurnOn();
                        break;
                    
                    case RoomStateValue.Sleep:
                        fans.Bedroom.SetPercentage(33);
                        lights.BedroomDownLight.TurnOff();
                        lights.BedLeds.TurnOn();
                        lights.NightstandLights.TurnOff();
                        break;
                }
            });
    }
}