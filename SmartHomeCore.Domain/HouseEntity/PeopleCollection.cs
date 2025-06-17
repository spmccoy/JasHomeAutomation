using SmartHomeCore.Domain.Common;
using SmartHomeCore.Domain.PersonEntity;

namespace SmartHomeCore.Domain.HouseEntity;

public class PeopleCollection
{
    private readonly List<Person> _people = [];
    
    public IReadOnlyCollection<Person> People => _people.AsReadOnly();
    public bool EveryoneIsHome => _people.All(a => a.CurrentState.IsHome());
    public bool EveryoneIsAway => _people.All(a => a.CurrentState.IsAway());
    
    public void AddPerson(PersonId personId, string name)
    {
        if (_people.Any(p => p.Id == personId))
        {
            throw new InvalidOperationException($"Person with ID '{personId}' already exists");
        }

        _people.Add(new Person(personId, name));
    }
    
    public Person GetPerson(PersonId id)
    {
        return _people.FirstOrDefault(p => p.Id == id)
            ?? throw new EntityNotFoundException(nameof(Person), id);
    }
}
