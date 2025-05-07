using MqttEntities.Models;
using NetDaemonApps.Extensions;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.Services;

public class ShawnRoomService(
    ILogger<ShawnRoomService> logger,
    SelectEntities selects,
    SunEntities sunEntities) : IShawnRoomService
{
    public void DetermineAndSetRoomState()
    {
        var position = sunEntities.Sun.CurrentSolarPosition();
        switch (position)
        {
            case Sun.SolarPosition.Unknown or Sun.SolarPosition.Unavailable:
                logger.LogWarning("Current solar position is unknown. Switching to 'Off' state.");
                selects.ShawnroomStateNetdaemon.SelectOff();
                break;
            case Sun.SolarPosition.AboveHorizon:
                selects.ShawnroomStateNetdaemon.SelectDay();
                break;
            case Sun.SolarPosition.BelowHorizon:
                selects.ShawnroomStateNetdaemon.SelectNight();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(position), $"Unexpected solar position: {position}");
        }
    }
}