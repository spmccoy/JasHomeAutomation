using NetDaemonApps.apps.MainRoom.Controls;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class MainRoomService(Entities entities, ISunService sunService, ILogger<MainRoomService> logger) : IMainRoomService
{
    private readonly SelectEntity _select = entities.Select.MainroomStateSelectNetdaemon;
    public void ApplyStateBasedOnSolarIllumination()
    {
        var illumination = sunService.GetCurrentSunState().CurrentSolarIllumination;
        
        switch (illumination)
        {
            case Sun.SolarIllumination.Unknown:
                logger.LogWarning("Current solar illumination is unknown. Switching to 'Off' state.");
                _select.SelectOption(RoomState.Off);
                break;
            case Sun.SolarIllumination.Day:
                _select.SelectOption(RoomState.Day);
                break;
            case Sun.SolarIllumination.Night:
                _select.SelectOption(RoomState.Night);
                break;
            case Sun.SolarIllumination.Dawn:
                _select.SelectOption(RoomState.Dawn);
                break;
            case Sun.SolarIllumination.Dusk:
                _select.SelectOption(RoomState.Dusk);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(illumination), $"Unexpected solar illumination: {illumination}");
        }
    }
}