using Domain.Entities;

namespace NetDaemonApps.Interfaces;

public interface ILightService
{
    void TurnOffAreaLights(HaArea areaName);
}