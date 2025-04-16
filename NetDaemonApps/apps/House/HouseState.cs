using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House;

public class HouseState
{
    public HouseState(IHouseService houseService)
    {
        houseService.Select.StateChanges()
            .Where(w => RoomStates.FromString(w.New?.State) == RoomStates.HomeSecure)
            .Subscribe(_ => houseService.HandleHomeSecure());
        
        houseService.Select.StateChanges()
            .Where(w => RoomStates.FromString(w.New?.State) == RoomStates.HomeUnsecured)
            .Subscribe(_ => houseService.HandleHomeUnsecured());
        
        houseService.Select.StateChanges()
            .Where(w => RoomStates.FromString(w.New?.State) == RoomStates.Away)
            .Subscribe(_ => houseService.HandleAway());
        
        houseService.Select.StateChanges()
            .Where(w => RoomStates.FromString(w.New?.State) == RoomStates.Sleep)
            .Subscribe(_ => houseService.HandleSleep());
    }
}