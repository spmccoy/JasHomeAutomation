using System.Collections.Generic;

namespace NetDaemonApps.Models;

/// <summary>
/// Represents the Sun entity, providing information regarding solar position and illumination state.
/// </summary>
public class Sun
{
    private const int WarningTimeInHours = -1; // Represents the alert window in hours before events

    private static readonly Dictionary<string, SolarPosition> StateMappings = new()
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

    public Sun(SunEntity sunEntity)
    {
        NextDawn = sunEntity.Attributes?.NextDawn;
        NextDusk = sunEntity.Attributes?.NextDusk;
        NextRising = sunEntity.Attributes?.NextRising;
        NextSetting = sunEntity.Attributes?.NextSetting;
        CurrentDateTimeUtc = DateTime.UtcNow;
        SunState = sunEntity.State;

        UpdateSolarStates();
    }

    public SolarPosition CurrentSolarPosition { get; private set; }
    public SolarIllumination CurrentSolarIllumination { get; private set; }
    
    public bool IsDark => CurrentSolarPosition == SolarPosition.BelowHorizon;

    private string? NextDawn { get; }
    private string? NextDusk { get; }
    private string? NextRising { get; }
    private string? NextSetting { get; }
    private string? SunState { get; set; }
    private DateTime CurrentDateTimeUtc { get; set; }

    private void UpdateSolarStates()
    {
        CurrentSolarPosition = DetermineSolarPosition(SunState);
        CurrentSolarIllumination = DetermineSolarIllumination();
    }

    private SolarPosition DetermineSolarPosition(string? state)
    {
        if (string.IsNullOrEmpty(state)) return SolarPosition.Unknown;

        return StateMappings.TryGetValue(state, out var position)
            ? position
            : throw new ArgumentOutOfRangeException(state, "Invalid sun state");
    }

    private SolarIllumination DetermineSolarIllumination()
    {
        if (!TryParseDate(NextDusk, out var nextDusk) ||
            !TryParseDate(NextRising, out var nextRising))
        {
            return SolarIllumination.Unknown;
        }

        var isDusk = IsEventNear(nextDusk);
        var isDawn = IsEventNear(nextRising);

        return isDusk ? SolarIllumination.Dusk
            : isDawn ? SolarIllumination.Dawn
            : CurrentSolarPosition == SolarPosition.AboveHorizon ? SolarIllumination.Day
            : SolarIllumination.Night;
    }

    private bool IsEventNear(DateTime eventTime)
    {
        return CurrentDateTimeUtc > eventTime.AddHours(WarningTimeInHours);
    }

    private static bool TryParseDate(string? date, out DateTime parsedDate)
    {
        return DateTime.TryParse(date, out parsedDate);
    }
}