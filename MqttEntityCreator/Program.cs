// See https://aka.ms/new-console-template for more information
using HomeAssistantGenerated;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.Runtime;
using NetDaemonApps;
using NetDaemonApps.DomainEntities;
using NetDaemonApps.Services;

const string containingNamespace = $"{nameof(NetDaemonApps)}.{nameof(NetDaemonApps.apps)}";

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((config) =>
    {
        config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
    })
    .UseNetDaemonAppSettings()
    .UseNetDaemonRuntime()
    .UseNetDaemonMqttEntityManagement()
    .ConfigureServices(services =>
    {
        services.AddTransient<ILightService, LightService>();
        services.AddHomeAssistantGenerated();
        services.AddMqttEntities();
    })
    .Build();

var mqttEntityManager = host.Services.GetService<IMqttEntityManager>();

if (mqttEntityManager is null)
{
    throw new NullReferenceException($"{nameof(IMqttEntityManager)} is null");
}

var entities = host.Services.GetServices<MqttEntity>();
foreach (var e in entities)
{
    await CreateMqttEntityAsync(mqttEntityManager, e);
}

return;

async Task CreateMqttEntityAsync(IMqttEntityManager entityManager, MqttEntity entity)
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
                new { options = mqttSelect.Options.Select(option => option.Key).ToArray() });
            break;
        
        case MqttSensor mqttSensor:
            await entityManager.CreateAsync(
                mqttSensor.Id,
                new EntityCreationOptions(Name: mqttSensor.DisplayName));
            break;
        
        default:
            throw new InvalidOperationException("Unknown entity type");
    }
}