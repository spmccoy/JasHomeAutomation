namespace NetDaemonApps.apps.ShawnRoom.Controls;

/// <summary>
/// Main switch for shawn's room that can determine what state the room should be in.
/// </summary>
public class MainSwitch : MqttSwitch
{
    private readonly Entities _entities;

    /// <summary>
    /// Main switch for shawn's room that can determine what state the room should be in.
    /// </summary>
    public MainSwitch(Entities entities) 
        : base("ShawnRoom", "main", "Shawn's Room Switch")
    {
        _entities = entities;

        PersistState = false;
    }

    protected override void HandleOff()
    {
        _entities.Select.ShawnroomStateNetdaemon.SelectOption(ShawnsRoomStateSelect.Off);
    }

    protected override void HandleOn()
    {
        var sun = new Sun(_entities.Sun.Sun.State);
        _entities.Select.ShawnroomStateNetdaemon.SelectOption(sun.CurrentState == Sun.State.Day
            ? ShawnsRoomStateSelect.Day
            : ShawnsRoomStateSelect.Night);
    }
}