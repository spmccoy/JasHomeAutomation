using NetDaemonApps.Models;

namespace NetDaemonApps.Extensions;

public static class SunEntityExtension
{
    public static bool IsDark(this SunEntity sunEntity)
    {
        return CurrentSolarPosition(sunEntity) == Sun.SolarPosition.BelowHorizon;
    }
    
    public static Sun.SolarPosition CurrentSolarPosition(this SunEntity sunEntity)
    {
        if (string.IsNullOrEmpty(sunEntity.State))
        {
            return Sun.SolarPosition.Unknown;
        }

        return Sun.StateMappings.TryGetValue(sunEntity.State, out var position)
            ? position
            : throw new ArgumentOutOfRangeException(sunEntity.State, "Invalid sun state");
    }

    public static (DateTime from, DateTime to) Dusk(this SunEntity sunEntity)
    {
        var dusk = DateTime.Parse(sunEntity.Attributes?.NextDusk ?? throw new InvalidOperationException());
        return (dusk.AddHours(-1), dusk);
    }
    
    public static (DateTime from, DateTime to) Dawn(this SunEntity sunEntity)
    {
        var dawn = DateTime.Parse(sunEntity.Attributes?.NextDawn ?? throw new InvalidOperationException());
        return (dawn, dawn.AddHours(1));
    }
    
    public static Sun.SolarIllumination CurrentSolarIllumination(this SunEntity sunEntity)
    {
        var now = DateTime.UtcNow;
        var dawn = Dawn(sunEntity);
        var dusk = Dusk(sunEntity);

        if (now >= dawn.from && now <= dawn.to)
        {
            return Sun.SolarIllumination.Dawn;
        }

        if (now >= dusk.from && now <= dusk.to)
        {
            return Sun.SolarIllumination.Dusk;
        }

        if (CurrentSolarPosition(sunEntity) is Sun.SolarPosition.AboveHorizon)
        {
            return Sun.SolarIllumination.Day;
        }

        if (CurrentSolarPosition(sunEntity) is Sun.SolarPosition.BelowHorizon)
        {
            return Sun.SolarIllumination.Night;
        }
        
        return Sun.SolarIllumination.Unknown;
    }
}