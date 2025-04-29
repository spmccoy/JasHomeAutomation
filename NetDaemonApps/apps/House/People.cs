using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class People
{
    public People(PersonEntities people, SwitchEntities switches, IShawnRoomService shawnRoomService, ButtonEntities buttons)
    {
        people.Shawn.StateChanges()
            .Subscribe(stateChange =>
            {
                var newState = stateChange.New?.State;
                switch (newState)
                {
                    case HaState.Away:
                        switches.ShawnroomStateNetdaemon.TurnOff();
                        buttons.HouseGarageDoorCloseNetdaemon.Press();
                        break;

                    case HaState.Home:
                        buttons.HouseGarageDoorOpenNetdaemon.Press();
                        break;
                }
            });
        
        people.Justin.StateChanges()
            .Subscribe(stateChange =>
            {
                var newState = stateChange.New?.State;
                switch (newState)
                {
                    case HaState.Away:
                        buttons.HouseGarageDoorCloseNetdaemon.Press();
                        break;

                    case HaState.Home:
                        buttons.HouseGarageDoorOpenNetdaemon.Press();
                        break;
                }
            });
    }
}