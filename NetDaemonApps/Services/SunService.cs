using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class SunService(Entities entities) : ISunService
{
    public bool IsDark => GetCurrentSunState().CurrentSolarPosition == Sun.SolarPosition.BelowHorizon;
    
    public Sun GetCurrentSunState()
    {
        return new Sun(entities.Sun.Sun, DateTime.UtcNow);
    }
}