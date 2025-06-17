using System.Linq;
using NetDaemon.HassModel.Entities;
using SmartHomeCore.Application.UseCases;
using SmartHomeCore.Domain.HouseEntity;

namespace SmartHomeCore.NetDaemonApps.apps;

[NetDaemonApp]
public class PersonSubscriber
{
    private readonly PersonLeftUseCase _personLeftUseCase;
    private readonly PersonArrivedHomeUseCase _personArrivedHomeUseCase;
    private const string Unavailable = "unavailable";
    private const string Unknown = "unknown";
    private const string Away = "away";
    private const string Home = "home";

    public PersonSubscriber(
        IHaContext ha, 
        House house, 
        PersonLeftUseCase personLeftUseCase, 
        PersonArrivedHomeUseCase personArrivedHomeUseCase)
    {
        _personLeftUseCase = personLeftUseCase;
        _personArrivedHomeUseCase = personArrivedHomeUseCase;
        
        var peopleEntities = ha.GetAllEntities()
            .Where(w => house.People.Any(a => a.Id == w.EntityId))
            .ToList();

        foreach (var peopleEntity in peopleEntities)
        {
            Subscribe(peopleEntity);
        }
    }

    private void Subscribe(Entity peopleEntity)
    {
        peopleEntity.StateChanges()
            .SubscribeAsync(async stateChange =>
            {
                var newState = stateChange.New?.State;
                var oldState = stateChange.Old?.State;

                if (IgnoreStateChange(oldState))
                {
                    return;
                }

                switch (newState)
                {
                    case Away:
                        _personLeftUseCase.PersonId = peopleEntity.EntityId;
                        await _personLeftUseCase.HandleAsync();
                        break;
                    
                    case Home:
                        _personArrivedHomeUseCase.PersonId = peopleEntity.EntityId;
                        await _personArrivedHomeUseCase.HandleAsync();
                        break;
                }
            });
    }
    
    private static bool IgnoreStateChange(string? oldState)
    {
        return oldState == Unknown || oldState == Unavailable || string.IsNullOrWhiteSpace(oldState);
    }
}