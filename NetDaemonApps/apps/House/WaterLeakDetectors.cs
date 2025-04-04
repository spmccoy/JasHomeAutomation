using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class WaterLeakDetectors
{
    private readonly Entities _entities;

    public WaterLeakDetectors(ISensorService sensorService, Entities entities)
    {
        _entities = entities;
        foreach (var leakSensor in sensorService.GetAllWaterLeakSensors())
        {
            leakSensor
                .StateChanges()
                .Where(w => w.New?.State == HaState.On)
                .Subscribe(_ => HandleOn());
        }
    }
    
    private void HandleOn()
    {
        _entities.Switch.HouseWaterLeakDetectedNetdaemon.TurnOn();
    }
}