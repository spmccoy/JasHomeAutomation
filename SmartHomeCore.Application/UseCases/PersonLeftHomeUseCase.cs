using Microsoft.Extensions.Logging;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.HouseEntity;
using SmartHomeCore.Domain.PersonEntity;

namespace SmartHomeCore.Application.UseCases;

public class PersonLeftHomeUseCase(House house, IDomainEventDispatcher domainEventDispatcher, ILogger<PersonLeftHomeUseCase> logger) 
    : UseCase(house, domainEventDispatcher, logger)
{
    private readonly House _house = house;
    private PersonId? PersonId { get; set; }

    public Task HandleAsync(PersonId personId)
    {
        PersonId = personId;
        return base.HandleAsync();
    }
    
    protected override Task ExecuteCoreLogicAsync()
    {
        _house.MarkPersonAsAway(PersonId ?? throw new NullReferenceException(nameof(PersonId)));
        return Task.CompletedTask;
    }
}