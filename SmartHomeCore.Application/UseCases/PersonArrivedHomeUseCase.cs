using Microsoft.Extensions.Logging;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.HouseEntity;
using SmartHomeCore.Domain.PersonEntity;

namespace SmartHomeCore.Application.UseCases;

public class PersonArrivedHomeUseCase(House house, IDomainEventDispatcher domainEventDispatcher, ILogger<PersonArrivedHomeUseCase> logger) 
    : UseCase(house, domainEventDispatcher, logger)
{
    private readonly House _house = house;
    private PersonId? PersonId { get; set; }

    public Task HandleAsync(PersonId personId)
    {
        PersonId = personId ?? throw new ArgumentNullException(nameof(personId));
        return base.HandleAsync();
    }

    protected override Task ExecuteCoreLogicAsync()
    {
        _house.MarkPersonAsHome(PersonId!);
        return Task.CompletedTask;
    }
}