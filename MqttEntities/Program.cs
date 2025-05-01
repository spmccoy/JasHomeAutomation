using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MqttEntities.Common;
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
        case MqttSelect mqttSelect:
            await entityManager.CreateAsync(
                mqttSelect.Id,
                new EntityCreationOptions(Name: mqttSelect.DisplayName),
                new { options = mqttSelect.Options.Select(option => option).ToArray() });
            break;
        
        default:
            if (entity is null)
            {
                throw new NullReferenceException();
            }
            
            await entityManager.CreateAsync(
                entity.Id,
                new EntityCreationOptions(Name: entity.DisplayName));
            break;
    }
}