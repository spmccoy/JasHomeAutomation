using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class Shawn
{
    private readonly Entities _entities;
    private readonly IPersonService _personService;

    public Shawn(Entities entities, IPersonService personService)
    {
        _entities = entities;
        _personService = personService;
        entities.Person.Shawn.StateChanges()
            .Subscribe(s => ProcessStateChange(s.New?.State));
    }

    private void ProcessStateChange(string? newState)
    {
        var shawn = new Person(newState);

        if (!shawn.IsHome)
        {
            _entities.Switch.NetdaemonShawnroomMain.TurnOff();
        }

        if (!_personService.IsAnyoneHome())
        {
            // TODO: set the house to away
        }
    }
}