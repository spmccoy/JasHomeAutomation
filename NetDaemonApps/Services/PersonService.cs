using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class PersonService(PersonEntities people, SwitchEntities switches) : IPersonService
{
    public bool ShawnHome => people.Shawn.State == HaState.Home;
    public bool JustinHome => people.Justin.State == HaState.Home;
    
    public bool AnyoneHome => ShawnHome || JustinHome;
    
   public bool DontDisturbShawn => switches.ShawnroomDndNetdaemon.State == HaState.On;
}