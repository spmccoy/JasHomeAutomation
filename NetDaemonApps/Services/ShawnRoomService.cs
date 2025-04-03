using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class ShawnRoomService(Entities entities, ISunService sunService, ILogger<ShawnRoomService> logger) : IShawnRoomService
{
    private readonly SelectEntity _select = entities.Select.ShawnroomStateNetdaemon;

    public void DetermineAndSetRoomState()
    {
        var position = sunService.GetCurrentSunState().CurrentSolarPosition;
        switch (position)
        {
            case Sun.SolarPosition.Unknown or Sun.SolarPosition.Unavailable:
                logger.LogWarning("Current solar position is unknown. Switching to 'Off' state.");
                _select.SelectOption(RoomState.Off);
                break;
            case Sun.SolarPosition.AboveHorizon:
                _select.SelectOption(RoomState.Day);
                break;
            case Sun.SolarPosition.BelowHorizon:
                _select.SelectOption(RoomState.Night);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(position), $"Unexpected solar position: {position}");
        }
    }
}