using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.PersonEntity;

public class Person : Entity, IHasState<PersonState>
{
    public string Name { get; }
    
    public PersonId Id { get; }
    
    public PersonState CurrentState { get; private set; } = PersonState.Unknown;

    public Person(PersonId personId, string name)
    {
        ArgumentNullException.ThrowIfNull(personId);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Person name cannot be empty", nameof(name));
        }
        
        Id = personId;
        Name = name;
    }
    
    public void MarkAsHome() => ChangeState(PersonState.Home);
    
    public void MarkAsAway() => ChangeState(PersonState.Away);
    
    public void MarkAsUnknown() => ChangeState(PersonState.Unknown);
    
    public void ChangeState(PersonState newState)
    {
        var oldState = CurrentState;

        if (oldState == newState)
        {
            return;
        }

        CurrentState = newState;
        AddDomainEvent(new PersonStateChangedEvent(Name, oldState, newState));
    }
}