using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using NetDaemon.HassModel.Entities;

namespace NetDaemonApps.Services;

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