namespace NetDaemonApps.apps.ShawnRoom.Controls;

/// <summary>
/// Main switch for shawn's room that can determine what state the room should be in.
/// </summary>
public class MainSwitch(Entities entities)
    : MqttSwitch("ShawnRoom", "main", "Shawn's Room Switch")
{
    protected override void HandleOff()
    {
        entities.Select.ShawnroomStateNetdaemon.SelectOption(ShawnsRoomState.Off);
    }

    protected override void HandleOn()
    {
        var sun = new Sun(entities.Sun.Sun.State);
        entities.Select.ShawnroomStateNetdaemon.SelectOption(sun.CurrentState == Sun.State.Day
            ? ShawnsRoomState.Day
            : ShawnsRoomState.Night);
    }
}