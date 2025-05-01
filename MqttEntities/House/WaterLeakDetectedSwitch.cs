using MqttEntities.Common;

namespace MqttEntities.House;

public class WaterLeakDetectedSwitch : MqttSwitch
{
    public WaterLeakDetectedSwitch() 
        : base("House", "water-leak-detected", "Water Leak detected")
    {
        InitialValue = Off;
    }
}