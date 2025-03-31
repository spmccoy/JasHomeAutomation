namespace NetDaemonApps.DomainEntities;

public class HaCommonState
{
    public static readonly HaCommonState Active = new("Active");   
    public static readonly HaCommonState InActive = new("Inactive");
    public static readonly HaCommonState On = new("on");
    public static readonly HaCommonState Off = new("off");
    
    private string Value { get; }

    private HaCommonState(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
}