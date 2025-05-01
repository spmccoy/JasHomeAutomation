namespace NetDaemonApps.Models;

public class Camera
{
    public Camera(string name, CameraEntity cameraEntity)
    {
        Name = name;
        CameraEntity = cameraEntity;
    }

    public string Name { get; init; }
    
    public BinarySensorEntity? PersonSensorEntity { get; init; }
    
    public BinarySensorEntity? VehicleSensorEntity { get; init; }
    
    public BinarySensorEntity? AnimalSensorEntity { get; init; }
    
    public CameraEntity CameraEntity { get; init; }

    public bool MonitoringForPeople => PersonSensorEntity != null;
    
    public bool MonitoringForVehicles => VehicleSensorEntity != null;
    
    public bool MonitoringForAnimals => AnimalSensorEntity != null;

    public bool AllowSnapShots;
}