using Domain.Entities;
using MqttEntities.Common;

namespace MqttEntities.House;

public class HouseStateSelect : MqttSelect
{
    public HouseStateSelect() 
        : base("House", "state", "House State")
    {
        AddOption(RoomStates.HomeSecure);
        AddOption(RoomStates.HomeUnsecured);
        AddOption(RoomStates.Away);
        AddOption(RoomStates.Sleep);
    }
}