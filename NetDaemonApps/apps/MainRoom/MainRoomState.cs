using System.Reactive.Concurrency;
using MqttEntities.Models;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Extensions;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.apps.MainRoom;

[NetDaemonApp]
public class MainRoomState
{
    private DateTime? _lastPersonDetection;
    
    public MainRoomState(
        IMainRoomService mainRoomService, 
        IScheduler scheduler, 
        MediaPlayerEntities mediaPlayers, 
        SelectEntities selects,
        BinarySensorEntities binarySensors)
    {
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Dusk.ToString())
            .Subscribe(_ => mainRoomService.HandleDusk());
        
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Dawn.ToString())
            .Subscribe(_ => mainRoomService.HandleDawn());
        
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Off.ToString())
            .Subscribe(_ => mainRoomService.HandleOff());
        
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Day.ToString())
            .Subscribe(_ => mainRoomService.HandleDay());
        
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Night.ToString())
            .Subscribe(_ => mainRoomService.HandleNight());
        
        mainRoomService.Select.StateChanges()
            .Where(w => w.New?.State == RoomStates.Tv.ToString())
            .Subscribe(_ => mainRoomService.HandleTv());
        
        binarySensors.MainPerson
            .StateChanges()
            .WhenStateIsFor(s => s?.State == HaState.On, TimeSpan.FromMicroseconds(500), scheduler)
            .Subscribe(_ =>
            {
                _lastPersonDetection = DateTime.Now;
                mainRoomService.DetermineAndSetRoomState();

                if (selects.HouseStateNetdaemon.State == RoomStates.Sleep.ToString())
                {
                    scheduler.Schedule(TimeSpan.FromMinutes(20), () =>
                    {
                        selects.MainroomStateSelectNetdaemon.SelectOff();
                    });
                }
            });
        
        scheduler.Schedule(TimeSpan.FromMinutes(60), () =>
        {
            if (_lastPersonDetection == null || mediaPlayers.LivingRoomTv.IsOn())
            {
                return;
            }
            
            var timeDifferenceInSeconds = (DateTime.Now - _lastPersonDetection.Value).TotalMinutes;

            if (timeDifferenceInSeconds > 60)
            {
                selects.ShawnroomStateNetdaemon.SelectOff();
            }
        });
    }
}