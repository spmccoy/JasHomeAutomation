namespace SmartHomeCore.Domain.Covers;

public static class CoverStateExtensions
{
    public static CoverState ToCoverState(this string? state) => state?.ToLower() switch
    {
        "unknown" => CoverState.Unknown,
        "unavailable" => CoverState.Unavailable,
        "closed" => CoverState.Closed,
        "open" => CoverState.Open,
        "opening" => CoverState.Opening,
        "closing" => CoverState.Closing,
        "stopped" => CoverState.Stopped,
        _ => throw new ArgumentException($"Unknown cover state: {state}", nameof(state))
    };
}