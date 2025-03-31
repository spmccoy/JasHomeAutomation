namespace NetDaemonApps.apps.ShawnRoom;

/// <summary>
/// Main switch for shawn's room that can determine what state the room should be in.
/// </summary>
public class MainSwitch(Entities entities)
    : MqttSwitch("ShawnRoom", "main", "Shawn's Room Switch")
{
    protected override void HandleOff()
    {
        entities.Select.NetdaemonShawnroomState.SelectOption(ShawnsRoomState.Off);
    }

    protected override void HandleOn()
    {
        var sun = new Sun(entities.Sun.Sun.State);
        entities.Select.NetdaemonShawnroomState.SelectOption(sun.CurrentState == Sun.State.Day
            ? ShawnsRoomState.Day
            : ShawnsRoomState.Night);
    }
}