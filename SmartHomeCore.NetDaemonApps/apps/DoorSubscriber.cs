using System.Linq;
using NetDaemon.HassModel.Entities;
using SmartHomeCore.Application.UseCases;
using SmartHomeCore.Domain.DoorEntity;
using SmartHomeCore.Domain.HouseEntity;
using SmartHomeCore.Infrastructure.Integrations.HomeDevices;

namespace SmartHomeCore.NetDaemonApps.apps;

[NetDaemonApp]
public class DoorSubscriber : SubscriberBase
{
    private readonly Predicate<StateChange> _stateChangedToClose = stateChange =>
        !IsInitialOrFaultyState(stateChange.Old?.State)
        && stateChange.New?.State.ToHomeAssistantState() == HomeAssistantState.Closed;
    
    private readonly Predicate<StateChange> _stateChangedToOpen = stateChange =>
        !IsInitialOrFaultyState(stateChange.Old?.State)
        && stateChange.New?.State.ToHomeAssistantState() == HomeAssistantState.Open;
    
    private readonly Predicate<StateChange> _stateChangedToLocked = stateChange =>
        !IsInitialOrFaultyState(stateChange.Old?.State)
        && stateChange.New?.State.ToHomeAssistantState() == HomeAssistantState.Locked;
    
    private readonly Predicate<StateChange> _stateChangedToUnlocked = stateChange =>
        !IsInitialOrFaultyState(stateChange.Old?.State)
        && stateChange.New?.State.ToHomeAssistantState() == HomeAssistantState.Unlocked;
    
    public DoorSubscriber(
        IHaContext ha, 
        House house, 
        DoorClosedUseCase doorClosedUseCase,
        DoorOpenedUseCase doorOpenedUseCase,
        DoorLockedUseCase doorLockedUseCase,
        DoorUnLockedUseCase doorUnLockedUseCase)
    {
        var doorEntities = ha.GetAllEntities()
            .Where(w => house.Doors.Any(a => a.Id == w.EntityId))
            .ToList();

        foreach (var doorEntity in doorEntities)
        {
            doorEntity.StateChanges()
                .Where(stateChange => _stateChangedToClose(stateChange))
                .SubscribeAsync(_ => doorClosedUseCase.HandleAsync(DoorId.Create(doorEntity.EntityId)));
            
            doorEntity.StateChanges()
                .Where(stateChange => _stateChangedToOpen(stateChange))
                .SubscribeAsync(_ => doorOpenedUseCase.HandleAsync(DoorId.Create(doorEntity.EntityId)));
            
            doorEntity.StateChanges()
                .Where(stateChange => _stateChangedToLocked(stateChange))
                .SubscribeAsync(_ => doorLockedUseCase.HandleAsync(DoorId.Create(doorEntity.EntityId)));
            
            doorEntity.StateChanges()
                .Where(stateChange => _stateChangedToUnlocked(stateChange))
                .SubscribeAsync(_ => doorUnLockedUseCase.HandleAsync(DoorId.Create(doorEntity.EntityId)));
        }
    }
}