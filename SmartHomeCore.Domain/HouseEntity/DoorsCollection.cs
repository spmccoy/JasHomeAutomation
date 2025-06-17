using SmartHomeCore.Domain.Common;
using SmartHomeCore.Domain.DoorEntity;

namespace SmartHomeCore.Domain.HouseEntity;

public class DoorsCollection
{
    private readonly List<Door> _doors = [];
    
    public IReadOnlyCollection<Door> Doors => _doors.AsReadOnly();
    public bool AllDoorsClosed => _doors.All(d => d.DoorState.IsClosed() || d.DoorState.IsUnsupported());
    public bool AllDoorsLocked => _doors.All(d => d.LockState.IsLocked() || d.DoorState.IsUnsupported());
    
    public void AddDoor(DoorId doorId, string name, bool openCloseSupported, bool lockSupported)
    {
        if (_doors.Any(d => d.Id == doorId))
        {
            throw new InvalidOperationException($"Door with ID '{doorId}' already exists");
        }
        
        _doors.Add(new Door(doorId, name, openCloseSupported, lockSupported));
    }

    public Door GetDoor(string id)
    {
        return _doors.FirstOrDefault(d => d.Id == id)
            ?? throw new InvalidOperationException($"Door with ID '{id}' does not exist");
    }
}
