namespace NetDaemonApps.apps.ShawnRoom.Controls;

public class DndSwitch(Entities entities, ILogger<DndSwitch> logger) 
    : MqttSwitch("ShawnRoom", "dnd", "Shawn's not disturb switch")
{
    private LightAttributes? LastKnownLightAttributes { get; set; }
    
    protected override void HandleOff()
    {
        if (LastKnownLightAttributes == null)
        {
            logger.LogWarning("The last known light attributes did not save. Turning light off instead.");
            entities.Light.HuePlayRight.TurnOff();
        }
        else
        {
            entities.Light.HuePlayRight.TurnOn(hsColor: LastKnownLightAttributes.HsColor);
        }
    }

    protected override void HandleOn()
    {
        LastKnownLightAttributes = entities.Light.HuePlayRight.Attributes;
        entities.Light.HuePlayRight.TurnOn(colorName: "Red");
    }
}