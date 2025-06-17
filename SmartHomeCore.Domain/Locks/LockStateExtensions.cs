namespace SmartHomeCore.Domain.Locks;

public static class LockStateExtensions
{
    public static LockState ToLockState(this string? state) => state?.ToLower() switch
    {
        "unknown" => LockState.Unknown,
        "unavailable" => LockState.Unavailable,
        "locked" => LockState.Locked,
        "unlocked" => LockState.Unlocked,
        "jammed" => LockState.Jammed,
        _ => throw new ArgumentException($"Unknown cover state: {state}", nameof(state))
    };
}