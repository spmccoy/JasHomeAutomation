using System.Diagnostics.CodeAnalysis;

namespace NetDaemonApps.apps.ShawnRoom;

/// <summary>
/// Applies room settings for a given state.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class StateSelect : MqttSelect
{
    private readonly Entities _entities;
    public const string Off = "Off";
    public const string Day = "Day";
    public const string Night = "Night";
    public const string Gaming = "Gaming";
    public const string SimRacing = "Sim Racing";
    
    public StateSelect(Entities entities) 
        : base(entities, "ShawnRoom", "state", "Shawn's Room State")
    {
        _entities = entities;
        AddOption(Off, HandleOff);
        AddOption(Day, HandleDay);
        AddOption(Night, HandleNight);
        AddOption(Gaming, HandleGaming);
        AddOption(SimRacing, HandleSimRacing);
    }
    
    private void HandleOff()
    {
        _entities.Fan.ShawnPurifier.SetPercentage(100);
    }
    
    private void HandleDay()
    {
        _entities.Scene.NetdaemonShawnroomDay.TurnOn();
        _entities.Fan.ShawnPurifier.SetPercentage(33);
    }
    
    private void HandleNight()
    {
        _entities.Scene.NetdaemonShawnroomDay.TurnOn();
        _entities.Fan.ShawnPurifier.SetPercentage(33);
    }
    
    private void HandleSimRacing()
    {
        _entities.Fan.ShawnPurifier.SetPercentage(33);
    }

    private void HandleGaming()
    {
        _entities.Fan.ShawnPurifier.SetPercentage(33);
    }
}