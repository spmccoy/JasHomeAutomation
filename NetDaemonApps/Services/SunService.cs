using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class SunService(Entities entities) : ISunService
{
    public bool IsDark => GetCurrentSunState().CurrentSolarPosition == Sun.SolarPosition.BelowHorizon;
    
    public Sun GetCurrentSunState()
    {
        var sunAttr = entities.Sun.Sun.Attributes;
        return new Sun(
            sunAttr?.NextDawn,
            sunAttr?.NextDusk,
            sunAttr?.NextRising,
            sunAttr?.NextSetting,
            entities.Sun.Sun.State,
            DateTime.UtcNow);
    }
}