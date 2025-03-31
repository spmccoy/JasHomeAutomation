namespace NetDaemonApps.DomainEntities;

public class HaArea
{
    public static readonly HaArea ShawnRoom = new("shawns_office");
    
    private string Value { get; }

    private HaArea(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
}