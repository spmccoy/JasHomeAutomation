using MqttEntities.Common;

namespace MqttEntities.ShawnsRoom;

public class DndSwitch() 
    : MqttSwitch("ShawnRoom", "dnd", "Shawn's not disturb switch")
{
}