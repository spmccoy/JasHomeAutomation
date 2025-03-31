using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class PersonService(Entities entities) : IPersonService
{
    public bool IsAnyoneHome()
    {
        var shawn = new HaPerson(entities.Person.Shawn.State);
        var justin = new HaPerson(entities.Person.Justin.State);

        return shawn.IsHome || justin.IsHome;
    }
}