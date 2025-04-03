using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class SunService(Entities entities) : ISunService
{
    public Sun GetCurrentSunState()
    {
        return new Sun(entities.Sun.Sun, DateTime.UtcNow);
    }
}