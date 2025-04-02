using NetDaemonApps.apps.House.Controls;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House.Devices;

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
            _entities.Switch.ShawnroomMainNetdaemon.TurnOff();
        }

        if (_personService.IsNoOneHome())
        {
            _entities.Select.HouseStateNetdaemon.SelectOption(HouseStateSelect.Away);
        }
    }
}