using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class PersonService(Entities entities) : IPersonService
{
    public bool IsAnyoneHome()
    {
        var shawn = new Person(entities.Person.Shawn.State);
        var justin = new Person(entities.Person.Justin.State);

        return shawn.IsHome || justin.IsHome;
    }

    public bool IsNoOneHome()
    {
        return !IsAnyoneHome();
    }

    public bool DontDisturbShawn => entities.Switch.ShawnroomDndNetdaemon.State == HaState.On;
}