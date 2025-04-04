namespace NetDaemonApps.Services;

public interface IHouseService
{
    void DetermineAndSetHouseState();
    void DetermineAndSetOutsideLights();
}