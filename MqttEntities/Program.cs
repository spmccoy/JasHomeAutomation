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

var entities = MqttEntityManager.GetSubtypeInstancesOfMqttEntity();
foreach (var e in entities)
{
    await CreateMqttEntityAsync(mqttEntityManager, e);
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
            await entityManager.CreateAsync(
                mqttCover.Id, 
                new EntityCreationOptions(Name: mqttCover.DisplayName),
                new { state_open = "OPEN", state_closed = "CLOSED" });
            break;
        
        default:
            throw new InvalidOperationException("Unknown entity type");
    }
}