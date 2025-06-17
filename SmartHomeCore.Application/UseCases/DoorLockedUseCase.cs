using Microsoft.Extensions.Logging;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.DoorEntity;
using SmartHomeCore.Domain.HouseEntity;

namespace SmartHomeCore.Application.UseCases;

public class DoorLockedUseCase(House house, IDomainEventDispatcher domainEventDispatcher, ILogger<DoorLockedUseCase> logger)
    : UseCase(house, domainEventDispatcher, logger)
{
    private readonly House _house = house;
    private DoorId? DoorId { get; set; }
    
    public Task HandleAsync(DoorId doorId)
    {
        DoorId = doorId ?? throw new ArgumentNullException(nameof(doorId));
        return base.HandleAsync();
    }
    
    protected override Task ExecuteCoreLogicAsync()
    {
        _house.MarkDoorAsLocked(DoorId!);
        return Task.CompletedTask;
    }
}