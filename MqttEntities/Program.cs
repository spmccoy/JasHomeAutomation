using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MqttEntities.Common;
using NetDaemon.Extensions.MqttEntityManager;
using SmartHomeCore.Application;
using SmartHomeCore.Domain.AreaEntity;
using SmartHomeCore.Domain.HouseEntity;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((config) =>
    {
        config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationLayer();
    })
    .UseNetDaemonMqttEntityManagement()
    .Build();

var mqttEntityManager = host.Services.GetService<IMqttEntityManager>();
if (mqttEntityManager is null)
{
    throw new NullReferenceException($"{nameof(IMqttEntityManager)} is null");
}

// TODO: clean up mqtt entities that are no longer in use

var house = host.Services.GetRequiredService<House>();

string?[] houseOptions = HouseState.AllStates.Select(s => s.ToString()).ToArray();
await CreateSelectAsync(house.SelectId, "House State", houseOptions);

foreach (var area in house.Areas)
{
    var areaOptions = AreaState.AllStates.Select(s => s.ToString()).ToArray();
    await CreateSelectAsync(area.SelectId, area.Name, areaOptions);
}

return;

async Task CreateSelectAsync(string id, string displayName, string?[] options)
{
    await mqttEntityManager.CreateAsync(
        id,
        new EntityCreationOptions(Name: displayName),
        new { options });
}


/*
if (mqttEntityManager is null)
{
    throw new NullReferenceException($"{nameof(IMqttEntityManager)} is null");
}

var entities = MqttEntityManager.GetSubtypeInstancesOfMqttEntity();
foreach (var e in entities)
{
    await CreateMqttEntityAsync(mqttEntityManager, e);
}

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
        
        case MqttSensor mqttSensor:
            await entityManager.CreateAsync(
                mqttSensor.Id,
                new EntityCreationOptions(Name: mqttSensor.DisplayName, DeviceClass: mqttSensor.DeviceClass));
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
}*/