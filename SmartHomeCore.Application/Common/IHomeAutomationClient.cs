namespace SmartHomeCore.Application.Common;

public interface IHomeAutomationClient
{
    Task CloseGarageAsync();
    Task OpenGarageAsync();
    Task LockFrontDoorAsync();
    Task SecureAllEntryPointsAsync();
}