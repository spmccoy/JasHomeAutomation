using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.HouseEntity;

public class HouseStateChangedEvent : DomainEvent
{
    public HouseState OldState { get; set; }
    
    public HouseState NewState { get; set; }
    
    public HouseStateChangedEvent(HouseState oldState, HouseState newState)
    {
        OldState = oldState;
        NewState = newState;
    }
}
