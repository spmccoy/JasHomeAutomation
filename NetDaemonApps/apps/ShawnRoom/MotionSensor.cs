using NetDaemon.HassModel.Entities;

namespace NetDaemonApps.apps.ShawnRoom;

[NetDaemonApp]
public class MotionSensor
{
    private readonly Entities _entities;

    public MotionSensor(Entities entities)
    {
        _entities = entities;
        
        entities.BinarySensor.ShawnOfficeHueMotionSensorMotion
            .StateChanges()
            .Where(e => e.New.IsOn())
            .Subscribe(_ => HandleOn());
    }

    private void HandleOn()
    {
        _entities.Switch.ShawnOfficeHueMotionSensorMotionSensorEnabled.TurnOff();
        _entities.Switch.NetdaemonShawnroomMain.TurnOn();
    }
}