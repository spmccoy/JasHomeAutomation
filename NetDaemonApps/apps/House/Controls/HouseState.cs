namespace NetDaemonApps.apps.House.Controls;

[NetDaemonApp]
public class HouseState : MqttSelect
{
    private readonly Entities _entities;
    public const string Home = "Home";
    public const string Away = "Away";
    public const string Sleep = "Sleep";
    
    public HouseState(Entities entities) 
        : base("House", "state", "House State")
    {
        _entities = entities;
        
        AddOption(Home, HandleHome);
        AddOption(Away, HandleAway);
        AddOption(Sleep, HandleSleep);
    }

    private void HandleHome()
    {
    }
    
    private void HandleAway()
    {
        _entities.Switch.ShawnroomMainNetdaemon.TurnOff();
    }

    private void HandleSleep()
    {
        _entities.Switch.ShawnroomMainNetdaemon.TurnOff();
    }
}