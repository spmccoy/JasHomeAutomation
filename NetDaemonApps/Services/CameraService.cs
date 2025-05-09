using System.Linq;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.Services;

public class CameraService(BinarySensorEntities binarySensors, CameraEntities cameras) : ICameraService
{
    public Camera[] GetAllCameras()
    {
        return
        [
            new Camera("Doorbell", cameras.DoorbellFluent)
            {
                PersonSensorEntity = binarySensors.DoorbellPerson,
                VehicleSensorEntity = binarySensors.DoorbellVehicle,
                AllowSnapShots = true
            },
            new Camera("Front", cameras.FrontFluent)
            {
                PersonSensorEntity = binarySensors.FrontPerson,
                VehicleSensorEntity = binarySensors.FrontVehicle,
                AnimalSensorEntity = binarySensors.FrontPet,
                AllowSnapShots = true
            },
            new Camera("North Side", cameras.EastSideFluent)
            {
                PersonSensorEntity = binarySensors.EastSidePerson,
                VehicleSensorEntity = binarySensors.EastSideVehicle,
                AllowSnapShots = true
            },
            new Camera("South Side", cameras.WestSideFluent)
            {
                PersonSensorEntity = binarySensors.WestSidePerson,
                VehicleSensorEntity = binarySensors.WestSideVehicle,
                AllowSnapShots = true
            },
            new Camera("Back", cameras.BackFluent)
            {
                VehicleSensorEntity = binarySensors.BackVehicle,
                AllowSnapShots = true
            },
        ];
    }

    public Camera[] GetAllMonitoringForPeople()
    {
        return GetAllCameras().Where(w => w.MonitoringForPeople).ToArray();
    }
    
    public Camera[] GetAllMonitoringForVehicles()
    {
        return GetAllCameras().Where(w => w.MonitoringForVehicles).ToArray();
    }
    
    public Camera[] GetAllMonitoringForAnimals()
    {
        return GetAllCameras().Where(w => w.MonitoringForAnimals).ToArray();
    }
    
    // Leaving these in as examples
    /*private IEnumerable<Entity> GetAllCameraPersonSensorEntities()
    {
        return haRegistry.Entities
            .Where(w => w.Id != null && 
                        w.Id.StartsWith(HaEntityType.BinarySensor.ToString()) && 
                        w.Id.EndsWith("_person"))
            .Select(s => haContext.Entity(s.Id ?? string.Empty));
    }
    
    private IEnumerable<Entity> GetAllCameraVehicleSensorEntities()
    {
        return haRegistry.Entities
            .Where(w => w.Id != null && 
                        w.Id.StartsWith(HaEntityType.BinarySensor.ToString()) && 
                        w.Id.EndsWith("_vehicle"))
            .Select(s => haContext.Entity(s.Id ?? string.Empty));
    }*/
}