using NetDaemonApps.Models;

namespace NetDaemonApps.Interfaces;

public interface ICameraService
{
    Camera[] GetAllCameras();
    Camera[] GetAllMonitoringForPeople();
    Camera[] GetAllMonitoringForVehicles();
    Camera[] GetAllMonitoringForAnimals();
}