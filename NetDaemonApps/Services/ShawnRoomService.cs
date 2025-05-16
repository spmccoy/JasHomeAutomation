using System.Threading.Tasks;
using MqttEntities.Models;
using MqttEntities.ShawnsRoom;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Extensions;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.Services;

public class ShawnRoomService(
    ILogger<ShawnRoomService> logger,
    SelectEntities selects,
    SunEntities sunEntities,
    BinarySensorEntities binarySensors,
    SensorEntities sensors,
    IPersonService personService,
    RemoteEntities remotes,
    ShawnsRoomOccupiedBinarySensor occupiedBinarySensor) : IShawnRoomService
{
    private const int MotionDetectionMinutes = 30;
    private const int DesktopActivityMinutes = 10;
    private const string LoginWindowState = "loginwindow";
    
    public void DetermineAndSetRoomState()
    {
        if (binarySensors.ShawnsroomOccupiedNetdaemon.IsOff())
        {
            selects.ShawnroomStateNetdaemon.SelectOff();
            return;
        }
        
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
    
    public async Task UpdateOccupancySensorAsync()
    {
        if (personService.ShawnHome)
        {
            await occupiedBinarySensor.UpdateOccupancyStateAsync(ShouldConsiderOccupiedWhenHome);
        }
        else
        {
            await occupiedBinarySensor.UpdateOccupancyStateAsync(ShouldConsiderOccupiedWhenAway);
        }
    }
    
    private bool HasRecentMotion => sensors.ShawnsroomLastMotionNetdaemon.IsRecent(TimeSpan.FromMinutes(MotionDetectionMinutes)) ?? false;
    
    private bool HasRecentDesktopActivity => sensors.DesktopTcfukdgDesktopTcfukdgLastactive.IsRecent(TimeSpan.FromMinutes(DesktopActivityMinutes)) ?? false;
    
    private bool IsMacbookActive => binarySensors.ShawnsMacbookProActive.Attributes?.Sleeping == true
                                    || binarySensors.ShawnsMacbookProActive.IsOn()
                                    || sensors.ShawnsMacbookProFrontmostApp.State != LoginWindowState;
    
    private bool IsTelevisionOn => remotes.ShawnSOfficeTv.IsOn();
    
    private bool ShouldConsiderOccupiedWhenHome => HasRecentMotion
                                                   || IsTelevisionOn
                                                   || IsMacbookActive
                                                   || HasRecentDesktopActivity;
    private bool ShouldConsiderOccupiedWhenAway => HasRecentMotion
                                                   || IsTelevisionOn;
}