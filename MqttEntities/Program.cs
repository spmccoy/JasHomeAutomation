using System.Reflection;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDaemon.Extensions.MqttEntityManager;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((config) =>
    {
        config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
    })
    .UseNetDaemonMqttEntityManagement()
    .Build();

var mqttEntityManager = host.Services.GetService<IMqttEntityManager>();

if (mqttEntityManager is null)
{
    throw new NullReferenceException($"{nameof(IMqttEntityManager)} is null");
}

var entities = GetSubtypeInstancesOfMqttEntity();
foreach (var e in entities)
{
    await CreateMqttEntityAsync(mqttEntityManager, e as MqttEntity);
}

return;

async Task CreateMqttEntityAsync(IMqttEntityManager entityManager, MqttEntity? entity)
{
    switch (entity)
    {
        case MqttSwitch mqttSwitch:
            await entityManager.CreateAsync(
                mqttSwitch.Id,
                new EntityCreationOptions(Name: mqttSwitch.DisplayName));
            break;

        case MqttSelect mqttSelect:
            await entityManager.CreateAsync(
                mqttSelect.Id,
                new EntityCreationOptions(Name: mqttSelect.DisplayName),
                new { options = mqttSelect.Options.Select(option => option).ToArray() });
            break;
        
        case MqttSensor mqttSensor:
            await entityManager.CreateAsync(
                mqttSensor.Id,
                new EntityCreationOptions(Name: mqttSensor.DisplayName));
            break;
        
        case MqttCover mqttCover:
            await entityManager.CreateAsync(mqttCover.Id, new EntityCreationOptions(Name: mqttCover.DisplayName));
            break;
        
        default:
            throw new InvalidOperationException("Unknown entity type");
    }
}

static object[] GetSubtypeInstancesOfMqttEntity()
{
    var mqttEntitySubtypes = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.IsSubclassOf(typeof(MqttEntity)) && !t.IsAbstract) // Find all non-abstract subclasses
        .ToArray();

    var instances = new List<object>();
    foreach (var type in mqttEntitySubtypes)
    {
        try
        {
            var instance = Activator.CreateInstance(type);
            if (instance != null)
            {
                instances.Add(instance); // Add the instance as the specific type
            }
        }
        catch
        {
            Console.WriteLine($"Could not instantiate type: {type.FullName}");
        }
    }

    return instances.ToArray();
}