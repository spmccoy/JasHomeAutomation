using System.Collections.Generic;
using System.Linq;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class SensorService(IHaRegistry haRegistry, IHaContext haContext) : ISensorService
{
    public IEnumerable<Entity> GetWaterLeakSensors()
    {
        return haRegistry.Entities
            .Where(w => w.Id != null && w.Id.Contains("sensor_wleak"))
            .Select(s => haContext.Entity(s.Id ?? string.Empty));
    }
}