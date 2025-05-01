using NetDaemonApps.Models;

namespace NetDaemonApps.Interfaces;

public interface ISunService
{
    bool IsDark { get; }
    Sun GetCurrentSunState();
}