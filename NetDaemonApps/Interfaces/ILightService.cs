using NetDaemonApps.Models;

namespace NetDaemonApps.Interfaces;

public interface ILightService
{
    void TurnOffAreaLights(HaArea areaName);
}