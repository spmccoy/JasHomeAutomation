using SmartHomeCore.Infrastructure.Integrations.HomeDevices;

namespace SmartHomeCore.NetDaemonApps.apps;

public class SubscriberBase
{
    private static bool IsInitialOrFaultyState(HomeAssistantState oldState) =>
        oldState is HomeAssistantState.Unknown or HomeAssistantState.Unavailable;
    
    protected static bool IsInitialOrFaultyState(string? oldState) => IsInitialOrFaultyState(oldState.ToHomeAssistantState());

}