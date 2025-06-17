using SmartHomeCore.Domain.DoorEntity;
using SmartHomeCore.Domain.HouseEntity;
using SmartHomeCore.Domain.PersonEntity;

namespace SmartHomeCore.Application.Services;

public class HouseService
{
    private readonly House _house;

    public HouseService(House house)
    {
        _house = house;
    }

    public void PersonLeft(PersonId personId) => _house.MarkPersonAsAway(personId);
    
    public void PersonCameHome(PersonId personId) => _house.MarkPersonAsHome(personId);

    public void DoorOpened(DoorId doorId) => _house.MarkDoorAsOpen(doorId);

    public void DoorClosed(DoorId doorId) => _house.MarkDoorAsClosed(doorId);

    public void DoorLocked(DoorId doorId) => _house.MarkDoorAsLocked(doorId);
    
    public void DoorUnlocked(DoorId doorId) => _house.MarkDoorAsUnlocked(doorId);
}