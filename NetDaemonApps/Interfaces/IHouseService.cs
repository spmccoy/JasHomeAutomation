namespace NetDaemonApps.Interfaces;

public interface IHouseService
{
    bool HouseSecure { get; }
    void DetermineAndSetHouseState();
    void DetermineAndSetOutsideLights();
    void HandleHomeSecure();
    void HandleHomeUnsecured();
    void HandleAway();
    void HandleSleep();
    SelectEntity Select { get; }
}