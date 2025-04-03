namespace NetDaemonApps.DomainEntities;

public class HaArea
{
    public static readonly HaArea ShawnRoom = new("shawns_office");
    public static readonly HaArea MainRoom = new("dining_room");
    public static readonly HaArea JustinsRoom = new("justins_office");
    public static readonly HaArea Bedroom = new("bedroom");
    public static readonly HaArea Outside = new("front_yard");
    
    private string Value { get; }

    private HaArea(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
}