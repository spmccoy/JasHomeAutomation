namespace NetDaemonApps.apps.ShawnRoom.Controls;

/// <summary>
/// Main switch for shawn's room that can determine what state the room should be in.
/// </summary>
public class ShawnsRoomStateSwitch : MqttSwitch
{
    private readonly Entities _entities;

    /// <summary>
    /// Main switch for shawn's room that can determine what state the room should be in.
    /// </summary>
    public ShawnsRoomStateSwitch(Entities entities) 
        : base("ShawnRoom", "state", "Shawn's Room State Switch")
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
        var sun = new Sun(_entities.Sun.Sun, DateTime.UtcNow);
        _entities.Select.ShawnroomStateNetdaemon.SelectOption(sun.CurrentSolarPosition == Sun.SolarPosition.AboveHorizon
            ? ShawnsRoomStateSelect.Day
            : ShawnsRoomStateSelect.Night);
    }
}