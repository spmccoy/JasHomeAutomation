using System.Collections.Generic;

namespace NetDaemonApps.Models;

public class Sun
{
    public static readonly Dictionary<string, SolarPosition> StateMappings = new()
    {
        {"above_horizon", SolarPosition.AboveHorizon},
        {"unknown", SolarPosition.Unknown},
        {"below_horizon", SolarPosition.BelowHorizon},
        {"unavailable", SolarPosition.Unavailable}
    };

    public enum SolarPosition
    {
        Unknown,
        Unavailable,
        AboveHorizon,
        BelowHorizon
    }

    public enum SolarIllumination
    {
        Unknown,
        Day,
        Night,
        Dawn,
        Dusk
    }
}