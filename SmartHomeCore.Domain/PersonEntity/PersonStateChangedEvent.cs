using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.PersonEntity;

public class PersonStateChangedEvent : DomainEvent
{
    public PersonState OldState { get; set; }
    
    public PersonState NewState { get; set; }
    
    public string PersonName { get; set; }
    
    public PersonStateChangedEvent(string personName, PersonState oldState, PersonState newState)
    {
        PersonName = personName;
        OldState = oldState;
        NewState = newState;
    }
}