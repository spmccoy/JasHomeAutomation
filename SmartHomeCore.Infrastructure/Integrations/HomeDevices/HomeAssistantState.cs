namespace SmartHomeCore.Infrastructure.Integrations.HomeDevices;

public enum HomeAssistantState
{
    Unknown,
    Unavailable,
    Locked,
    Unlocked,
    Jammed,
    Home,
    Away,
    Closed,
    Open,
    Opening,
    Closing,
    Stopped
}

public static class HomeAssistantStatesExtensions
{
    public static HomeAssistantState ToHomeAssistantState(this string? state) => state?.ToLower() switch
    {
        "unknown" => HomeAssistantState.Unknown,
        "unavailable" => HomeAssistantState.Unavailable,
        "home" => HomeAssistantState.Home,
        "away" => HomeAssistantState.Away,
        "closed" => HomeAssistantState.Closed,
        "open" => HomeAssistantState.Open,
        "opening" => HomeAssistantState.Opening,
        "closing" => HomeAssistantState.Closing,
        "stopped" => HomeAssistantState.Stopped,
        "locked" => HomeAssistantState.Locked,
        "unlocked" => HomeAssistantState.Unlocked,
        "jammed" => HomeAssistantState.Jammed,
        _ => throw new ArgumentException($"Unknown person state: {state}", nameof(state))
    };
}