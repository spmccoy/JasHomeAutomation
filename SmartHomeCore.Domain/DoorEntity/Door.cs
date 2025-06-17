using SmartHomeCore.Domain.Common;

namespace SmartHomeCore.Domain.DoorEntity;

public class Door : Entity
{
    public DoorId Id { get; }
    
    public string Name { get; }

    public Door(DoorId doorId, string name, bool openCloseSupported, bool lockSupported)
    {
        ArgumentNullException.ThrowIfNull(doorId);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(name);
        }
        
        Id = doorId;
        Name = name;
        DoorState = openCloseSupported ? DoorState.Unknown : DoorState.Unsupported;
        LockState = lockSupported ? LockState.Unknown : LockState.Unsupported;
    }
    
    public DoorState DoorState { get; private set; }
    public LockState LockState { get; private set; }
    
    public void MarkAsOpen() => ChangeDoorState(DoorState.Open);
    
    public void MarkAsClosed() => ChangeDoorState(DoorState.Closed);
    
    public void MarkAsDoorStateUnknown() => ChangeDoorState(DoorState.Unknown);
    
    public void MarkAsLocked() => ChangeLockState(LockState.Locked);
    
    public void MarkAsUnlocked() => ChangeLockState(LockState.Unlocked);
    
    public void MarkAsLockStateUnknown() => ChangeLockState(LockState.Unknown);
    
    private void ChangeLockState(LockState newState)
    {
        var oldState = LockState;

        if (oldState == newState)
        {
            return;
        }

        LockState = newState;
        AddDomainEvent(new DoorLockStateChangedEvent(newState, oldState));
    }

    private void ChangeDoorState(DoorState newState)
    {
        var oldState = DoorState;

        if (oldState == newState)
        {
            return;
        }

        DoorState = newState;
        AddDomainEvent(new DoorStateChangedEvent(newState, oldState));
    }
}