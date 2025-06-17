namespace SmartHomeCore.Infrastructure.Common;

public interface IGarage
{
    Task CloseDoorAsync();
    Task OpenDoorAsync();
}