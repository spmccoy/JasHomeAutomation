using System.Threading.Tasks;

namespace NetDaemonApps.Interfaces;

public interface IShawnRoomService
{
    SelectEntity Select { get; }
    SwitchEntity Switch { get; }
    void DetermineAndSetRoomState();
    Task HandleOffAsync();
    void HandleDay();
    void HandleNight();
    void HandleSimRacing();
    void HandleGaming();
    bool AreSwitchAndSelectStatesInSync();
    void SyncSwitchToSelect();
    void HandleSwitch();
}