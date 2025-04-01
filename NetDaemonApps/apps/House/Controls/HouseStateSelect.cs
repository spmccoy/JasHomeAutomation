namespace NetDaemonApps.apps.House.Controls;

[NetDaemonApp]
public class HouseStateSelect : MqttSelect
{
    private readonly Entities _entities;
    public const string HomeSecure = "Home Secure";
    public const string HomeUnsecured = "Home Unsecured";
    public const string Away = "Away";
    public const string Sleep = "Sleep";
    
    public HouseStateSelect(Entities entities) 
        : base("House", "state", "House State")
    {
        _entities = entities;
        
        AddOption(HomeSecure, HandleHomeSecure);
        AddOption(HomeUnsecured, HandleHomeUnsecured);
        AddOption(Away, HandleAway);
        AddOption(Sleep, HandleSleep);
    }

    private void HandleHomeSecure()
    {
    }
    
    private void HandleHomeUnsecured()
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