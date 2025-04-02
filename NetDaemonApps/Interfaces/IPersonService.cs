namespace NetDaemonApps.Interfaces;

public interface IPersonService
{
    bool IsAnyoneHome();
    bool IsNoOneHome();

    bool DontDisturbShawn { get; }
}