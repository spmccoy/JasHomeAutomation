using Domain.Entities;

namespace NetDaemonApps.Interfaces;

public interface ISunService
{
    bool IsDark { get; }
    Sun GetCurrentSunState();
}