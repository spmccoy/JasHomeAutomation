using Domain.Entities;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class DoNotDisturb
{
    private LightAttributes? LastKnownLightAttributes { get; set; }
    
    public DoNotDisturb(SwitchEntities switchEntities, LightEntities lights, ILogger<DoNotDisturb> logger)
    {
        switchEntities.ShawnroomDndNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.On)
            .Subscribe(_ =>
            {
                LastKnownLightAttributes = lights.HuePlayRight.Attributes;
                lights.HuePlayRight.TurnOn(colorName: "Red");
            });
        
        switchEntities.ShawnroomDndNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.Off)
            .Subscribe(_ =>
            {
                if (LastKnownLightAttributes == null)
                {
                    logger.LogWarning("The last known light attributes did not save. Turning light off instead.");
                    lights.HuePlayRight.TurnOff();
                }
                else
                {
                    if (lights.HuePlayRight.Attributes?.ColorMode != "xy")
                    {
                        logger.LogError("The lights do not have the proper color mode. Turning light off instead.");
                        lights.HuePlayRight.TurnOff();
                        return;
                    }
                    lights.HuePlayRight.TurnOn(xyColor: LastKnownLightAttributes.XyColor);
                }
            });
    }
}