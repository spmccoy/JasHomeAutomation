namespace NetDaemonApps.Interfaces;

public interface IMainRoomService
{
    SelectEntity Select { get; }
    SwitchEntity Switch { get; }
    void DetermineAndSetRoomState();
    void HandleDusk();
    void HandleDawn();
    void HandleOff();
    void HandleDay();
    void HandleNight();
    void HandleTv();
}