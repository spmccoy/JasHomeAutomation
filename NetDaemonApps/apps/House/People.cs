using System.Threading;
using System.Threading.Tasks;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.HassModel.Entities;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class People : MqttSensor, IAsyncInitializable
{
    private readonly Entities _entities;
    private readonly IMqttEntityManager _mqttEntityManager;

    public bool IsAnyoneHome => IsShawnHome || IsJustinHome;
    public bool IsShawnHome => _entities.Person.Shawn.State == HaPerson.Home.ToString();
    public bool IsJustinHome => _entities.Person.Justin.State == HaPerson.Home.ToString();

    public People(Entities entities, IMqttEntityManager mqttEntityManager)
        : base("House", "is_anyone_home", "Is anyone home")
    {
        _entities = entities;
        _mqttEntityManager = mqttEntityManager;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await UpdateSensorValue();
        
        _entities.Person.Shawn.StateChanges()
            .Subscribe(ProcessShawnState);
        
        _entities.Person.Justin.StateChanges()
            .Subscribe(ProcessJustinState);
    }

    public async Task UpdateSensorValue()
    { 
        await _mqttEntityManager.SetStateAsync(Id, MqttSensor.StringValueFromBool(IsAnyoneHome));
    }
    
    private async void ProcessShawnState(StateChange<PersonEntity, EntityState<PersonAttributes>> stateChange)
    {
        await AnyStateChange();
        if (!IsShawnHome)
        {
            _entities.Switch.NetdaemonShawnroomMain.TurnOff();
        }
    }

    private async void ProcessJustinState(StateChange<PersonEntity, EntityState<PersonAttributes>> stateChange)
    {
        await AnyStateChange();
    }

    private async Task AnyStateChange()
    {
        await UpdateSensorValue();
        if (!IsAnyoneHome)
        {
            // TODO: change house state to away    
        }
    }

    protected override void HandleOff()
    {
    }

    protected override void HandleOn()
    {
    }
}