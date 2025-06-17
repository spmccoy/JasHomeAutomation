using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.DoorEntity;

public class DoorStateChangedEvent : DomainEvent
{
    public DoorState OldState { get; set; }
    
    public DoorState NewState { get; set; }
    
    public DoorStateChangedEvent(DoorState oldState, DoorState newState)
    {
        OldState = oldState;
        NewState = newState;
    }
}