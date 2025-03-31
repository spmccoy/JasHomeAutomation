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
        entities.Select.NetdaemonShawnroomState.SelectOption(entities.Sun.Sun.State == HaSun.AboveHorizon.ToString()
            ? ShawnsRoomState.Day
            : ShawnsRoomState.Night);
    }
}