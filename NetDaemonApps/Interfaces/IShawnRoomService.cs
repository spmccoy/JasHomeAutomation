using System.Threading.Tasks;

namespace NetDaemonApps.Interfaces;

public interface IShawnRoomService
{
    void DetermineAndSetRoomState();

    Task UpdateOccupancySensorAsync();
}