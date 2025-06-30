using SmartHomeCore.Domain.Common;
using SmartHomeCore.Domain.SceneEntity;

namespace SmartHomeCore.Domain.AreaEntity;

public class AreaStateChangedEvent : DomainEvent
{
    public AreaStateChangedEvent(string name, AreaState oldState, AreaState newState, Scene? scene)
    {
        Name = name;
        OldState = oldState;
        NewState = newState;
        Scene = scene;
    }
    
    public string Name { get; }
    
    public AreaState OldState { get; }
    
    public AreaState NewState { get; }
    
    public Scene? Scene { get; }
}