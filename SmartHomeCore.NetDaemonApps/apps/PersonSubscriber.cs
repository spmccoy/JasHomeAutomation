using System.Linq;
using NetDaemon.HassModel.Entities;
using SmartHomeCore.Application.UseCases;
using SmartHomeCore.Domain.HouseEntity;
using SmartHomeCore.Domain.PersonEntity;

namespace SmartHomeCore.NetDaemonApps.apps;

[NetDaemonApp]
public class PersonSubscriber
{
    private readonly PersonLeftHomeUseCase _personLeftHomeUseCase;
    private readonly PersonArrivedHomeUseCase _personArrivedHomeUseCase;
    private const string Unavailable = "unavailable";
    private const string Unknown = "unknown";
    private const string Away = "away";
    private const string Home = "home";

    public PersonSubscriber(
        IHaContext ha, 
        House house, 
        PersonLeftHomeUseCase personLeftHomeUseCase, 
        PersonArrivedHomeUseCase personArrivedHomeUseCase)
    {
        _personLeftHomeUseCase = personLeftHomeUseCase;
        _personArrivedHomeUseCase = personArrivedHomeUseCase;
        
        var peopleEntities = ha.GetAllEntities()
            .Where(w => house.People.Any(a => a.Id == w.EntityId))
            .ToList();

        foreach (var peopleEntity in peopleEntities)
        {
            Subscribe(peopleEntity);
        }
    }

    private static bool IgnoreStateChange(string? oldState) => 
        oldState == Unknown || oldState == Unavailable || string.IsNullOrWhiteSpace(oldState);
    
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
                        _personLeftHomeUseCase.PersonId = PersonId.Create(peopleEntity.EntityId);
                        await _personLeftHomeUseCase.HandleAsync();
                        break;
                    
                    case Home:
                        _personArrivedHomeUseCase.PersonId = PersonId.Create(peopleEntity.EntityId);
                        await _personArrivedHomeUseCase.HandleAsync();
                        break;
                }
            });
    }
}