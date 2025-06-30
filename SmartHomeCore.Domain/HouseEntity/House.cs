using SmartHomeCore.Domain.Common;
using SmartHomeCore.Domain.DoorEntity;
using SmartHomeCore.Domain.PersonEntity;
using SmartHomeCore.Domain.AreaEntity;

namespace SmartHomeCore.Domain.HouseEntity;

public class House : Entity, IHasState<HouseState>
{
    private readonly PeopleCollection _peopleCollection;
    private readonly DoorsCollection _doorsCollection;
    private readonly AreasCollection _areasCollection;

    public House()
    {
        _peopleCollection = new PeopleCollection();
        _doorsCollection = new DoorsCollection();
        _areasCollection = new AreasCollection();
    }
    
    public HouseState CurrentState { get; private set; } = HouseState.Unknown;

    public IReadOnlyCollection<Person> People => _peopleCollection.People;
    public IReadOnlyCollection<Door> Doors => _doorsCollection.Doors;
    public IReadOnlyCollection<Area> Areas => _areasCollection.Areas;
    
    public string SelectId => GenerateSelectId("House");

    // House state properties derived from collections
    public bool EveryoneIsHome => _peopleCollection.EveryoneIsHome;
    public bool EveryoneIsAway => _peopleCollection.EveryoneIsAway;
    public bool AllDoorsClosed => _doorsCollection.AllDoorsClosed;
    public bool AllDoorsLocked => _doorsCollection.AllDoorsLocked;

    // House state methods
    public void MarkAsSecure() => ChangeState(HouseState.Secure);
    public void MarkAsUnsecure() => ChangeState(HouseState.Unsecure);
    public void MarkAsEveryoneAway() => ChangeState(HouseState.EveryoneAway);
    public void MarkAsSleep() => ChangeState(HouseState.Sleep);

    // Door management methods
    public void AddDoor(DoorId doorId, string name, bool openCloseSupported, bool lockSupported) =>
        _doorsCollection.AddDoor(doorId, name, openCloseSupported, lockSupported);
    
    public void MarkDoorAsOpen(DoorId id)
    {
        var door = _doorsCollection.GetDoor(id);
        door.MarkAsOpen();
    }

    public void MarkDoorAsClosed(DoorId id)
    {
        var door = _doorsCollection.GetDoor(id);
        door.MarkAsClosed();
    }

    public void MarkDoorAsLocked(DoorId id)
    {
        var door = _doorsCollection.GetDoor(id);
        door.MarkAsLocked();
    }

    public void MarkDoorAsUnlocked(DoorId id)
    {
        var door = _doorsCollection.GetDoor(id);
        door.MarkAsUnlocked();
    }
    
    // People management methods
    public void AddPerson(PersonId personId, string name) => _peopleCollection.AddPerson(personId, name);
    public Person? GetPerson(PersonId id) => _peopleCollection.GetPerson(id);
    
    public void MarkPersonAsHome(PersonId id)
    {
        var person = _peopleCollection.GetPerson(id);
        person.MarkAsHome();
    }
    
    public void MarkPersonAsAway(PersonId id)
    {
        var person = _peopleCollection.GetPerson(id);
        person.MarkAsAway();
    }
    
    public void ChangeState(HouseState newState)
    {
        var oldState = CurrentState;

        if (oldState == newState)
        {
            return;
        }

        CurrentState = newState;
        AddDomainEvent(new HouseStateChangedEvent(oldState, newState));
    }
    
    // Area management methods
    public Area AddArea(AreaId areaId, string name) => _areasCollection.AddArea(areaId, name);
    public Area GetArea(AreaId id) => _areasCollection.GetArea(id);
    public Area GetAreaBySelectId(string selectId) => _areasCollection.GetAreaBySelectId(selectId);
    
    public void ApplyHouseRules()
    {
        if (EveryoneIsAway)
        {
            MarkAsEveryoneAway();
            return;
        }

        if (AllDoorsClosed && AllDoorsLocked)
        {
            MarkAsSecure();
        }
        else
        {
            MarkAsUnsecure();
        }
    }
}