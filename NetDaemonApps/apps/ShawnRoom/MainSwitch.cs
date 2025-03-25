namespace NetDaemonApps.apps.ShawnRoom;

/// <summary>
/// Main switch for shawn's room that can determine what state the room should be in.
/// </summary>
public class MainSwitch(Entities entities)
    : MqttSwitch(entities, "ShawnRoom", "main", "Shawn's Room Switch")
{
    private readonly Entities _entities = entities;

    public override void HandleOff()
    {
        _entities.Select.NetdaemonShawnroomState.SelectOption(StateSelect.Off);
    }

    public override void HandleOn()
    {
        _entities.Select.NetdaemonShawnroomState.SelectOption(_entities.Sun.Sun.State == HaSun.AboveHorizon.ToString()
            ? StateSelect.Day
            : StateSelect.Night);
    }
}