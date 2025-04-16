using Domain.Entities;
using NetDaemon.HassModel.Entities;

namespace NetDaemonApps.apps.ShawnRoom.Devices;

[NetDaemonApp]
public class DoNotDisturb
{
    private readonly Entities _entities;
    private readonly ILogger<DoNotDisturb> _logger;
    private LightAttributes? LastKnownLightAttributes { get; set; }
    
    public DoNotDisturb(SwitchEntities switchEntities, Entities entities, ILogger<DoNotDisturb> logger)
    {
        _entities = entities;
        _logger = logger;
        switchEntities.ShawnroomDndNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.On)
            .Subscribe(_ => HandleOn());
        
        switchEntities.ShawnroomDndNetdaemon.StateChanges()
            .Where(w => w.New?.State == HaState.On)
            .Subscribe(_ => HandleOff());
    }
    
    private void HandleOff()
    {
        if (LastKnownLightAttributes == null)
        {
            _logger.LogWarning("The last known light attributes did not save. Turning light off instead.");
            _entities.Light.HuePlayRight.TurnOff();
        }
        else
        {
            _entities.Light.HuePlayRight.TurnOn(hsColor: LastKnownLightAttributes.HsColor);
        }
    }

    private void HandleOn()
    {
        LastKnownLightAttributes = _entities.Light.HuePlayRight.Attributes;
        _entities.Light.HuePlayRight.TurnOn(colorName: "Red");
    }
}