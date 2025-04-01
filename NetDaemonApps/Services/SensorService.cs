using System.Collections.Generic;
using System.Linq;
using NetDaemon.HassModel.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class SensorService(IHaRegistry haRegistry, IHaContext haContext) : ISensorService
{
    public IEnumerable<Entity> GetAllWaterLeakSensors()
    {
        return haRegistry.Entities
            .Where(w => w.Id != null && w.Id.Contains("sensor_wleak"))
            .Select(s => haContext.Entity(s.Id ?? string.Empty));
    }
}

public class CameraService(IHaRegistry haRegistry, IHaContext haContext)
{
    public IEnumerable<Entity> GetAllCameraPersonSensorEntities()
    {
        return haRegistry.Entities
            .Where(w => w.Id != null && 
                        w.Id.StartsWith(HaEntityType.BinarySensor.ToString()) && 
                        w.Id.EndsWith("_person"))
            .Select(s => haContext.Entity(s.Id ?? string.Empty));
    }
    
    public IEnumerable<Entity> GetAllCameraVehicleSensorEntities()
    {
        return haRegistry.Entities
            .Where(w => w.Id != null && 
                        w.Id.StartsWith(HaEntityType.BinarySensor.ToString()) && 
                        w.Id.EndsWith("_vehicle"))
            .Select(s => haContext.Entity(s.Id ?? string.Empty));
    }
}