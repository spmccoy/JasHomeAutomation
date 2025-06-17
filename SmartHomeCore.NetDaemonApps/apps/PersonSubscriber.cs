using System.Linq;
using NetDaemon.HassModel.Entities;
using SmartHomeCore.Application.UseCases;
using SmartHomeCore.Domain.HouseEntity;
using SmartHomeCore.Domain.PersonEntity;
using SmartHomeCore.Infrastructure.Integrations.HomeDevices;

namespace SmartHomeCore.NetDaemonApps.apps;

[NetDaemonApp]
public class PersonSubscriber : SubscriberBase
{
    private readonly Predicate<StateChange> _stateChangedToAway = stateChange =>
        !IsInitialOrFaultyState(stateChange.Old?.State)
        && stateChange.New?.State.ToHomeAssistantState() == HomeAssistantState.Away;
    
    private readonly Predicate<StateChange> _stateChangedToHome = stateChange =>
        !IsInitialOrFaultyState(stateChange.Old?.State)
        && stateChange.New?.State.ToHomeAssistantState() == HomeAssistantState.Home;

    public PersonSubscriber(
        IHaContext ha, 
        House house, 
        PersonLeftHomeUseCase personLeftHomeUseCase, 
        PersonArrivedHomeUseCase personArrivedHomeUseCase)
    {
        var peopleEntities = ha.GetAllEntities()
            .Where(w => house.People.Any(a => a.Id == w.EntityId))
            .ToList();

        foreach (var peopleEntity in peopleEntities)
        {
            peopleEntity.StateChanges()
                .Where(stateChange => _stateChangedToAway(stateChange))
                .SubscribeAsync(_ => personLeftHomeUseCase.HandleAsync(PersonId.Create(peopleEntity.EntityId)));
            
            peopleEntity.StateChanges()
                .Where(stateChange => _stateChangedToHome(stateChange))
                .SubscribeAsync(_ => personArrivedHomeUseCase.HandleAsync(PersonId.Create(peopleEntity.EntityId)));
        }
    }
}