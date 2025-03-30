namespace NetDaemonApps.DomainEntities;

public class HaCommonState
{
    public static readonly HaCommonState Active = new("Active");
    
    public static readonly HaCommonState InActive = new("Inactive");
    
    private string Value { get; }

    private HaCommonState(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
}