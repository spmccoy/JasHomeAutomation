using System.Threading.Tasks;
using MqttEntities.Models;
using MQTTnet.Server;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.JustinRoom;

[NetDaemonApp]
public class JustinsRoomState
{
    public JustinsRoomState(SelectEntities selects, FanEntities fans, LightEntities lights, ScheduleEntities calendars, IPersonService personService)
    {
        selects.JustinroomStateNetdaemon.StateChanges()
            .Subscribe(stateChange =>
            {
                var newState = RoomStates.FromString(stateChange.New?.State).Value;

                switch (newState)
                {
                    case RoomStateValue.Day:
                        fans.JustinPurifier.SetPercentage(33);
                        fans.JustinsOffice.TurnOn();
                        lights.JustinsOffice.TurnOn();
                        break;
                    
                    case RoomStateValue.Off:
                        fans.JustinPurifier.SetPercentage(100);
                        fans.JustinsOffice.TurnOff();
                        lights.JustinsOffice.TurnOff();
                        break;
                }
            });
        
        calendars.JustinOfficeHours.StateChanges()
            .Subscribe(stateChange =>
            {
                var newState = stateChange.New?.State;

                switch (newState)
                {
                    case HaState.On when personService.JustinHome:
                        selects.JustinroomStateNetdaemon.SelectOption(RoomStates.Day.ToString());
                        break;
                    
                    case HaState.Off:
                        selects.JustinroomStateNetdaemon.SelectOption(RoomStates.Off.ToString());
                        break;
                }
            });
    }
}