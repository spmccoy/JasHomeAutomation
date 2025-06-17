using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.HouseEntity;

namespace SmartHomeCore.Application.UseCases;

public class PersonArrivedHomeUseCase(House house, IDomainEventDispatcher domainEventDispatcher) 
    : UseCase(house, domainEventDispatcher)
{
    private readonly House _house = house;
    public string? PersonId { get; set; }

    protected override Task ExecuteCoreLogicAsync()
    {
        _house.MarkPersonAsHome(PersonId ?? throw new NullReferenceException(PersonId));
        return Task.CompletedTask;
    }
}