using System.Linq;

namespace NetDaemonApps.Services;

public class LightService(IHaRegistry haRegistry, ILogger<LightService> logger) : ILightService
{
    public void TurnOffAreaLights(HaArea areaName)
    {
        var area = haRegistry.GetArea(areaName.ToString());

        if (area == null)
        {
            logger.LogError("Area {Area} not found", areaName);
            return;
        }

        var entities = area.Entities
            .Where(w => w.EntityId.StartsWith(HaEntityType.Light.ToString()))
            .ToList();
        
        entities.ForEach(light => light.CallService("turn_off"));
    }
}