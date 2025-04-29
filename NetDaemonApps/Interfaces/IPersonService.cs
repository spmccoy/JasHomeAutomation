namespace NetDaemonApps.Interfaces;

public interface IPersonService
{
    bool ShawnHome { get; }
    bool JustinHome { get; }
    bool AnyoneHome { get; }
    bool DontDisturbShawn { get; }
}