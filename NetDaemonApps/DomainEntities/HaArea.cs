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

public class HaPerson
{
    // zone_name – If the person is detected in a defined zone, their state will be the name of that zone (e.g., work, gym, school).
    public static readonly HaPerson Home = new("home");
    public static readonly HaPerson NotHome = new("not_home");
    public static readonly HaPerson Unknown = new("unknown");
    public static readonly HaPerson Unavailable = new("unavailable");
    
    private string Value { get; }

    private HaPerson(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
}