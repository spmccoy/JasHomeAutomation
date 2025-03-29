// See https://aka.ms/new-console-template for more information

using HomeAssistantGenerated;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.Runtime;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Services;

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
        // Register your services here
        services.AddScoped<IMqttSwitchService, MqttSwitchService>();
        services.AddScoped<IMqttSelectService, MqttSelectService>();
        services.AddHomeAssistantGenerated();
    })
    .Build();

var mqttSwitchService = host.Services.GetRequiredService<IMqttSwitchService>();
await mqttSwitchService.CreateAllAsync();

var mqttSelectService = host.Services.GetRequiredService<IMqttSelectService>();
await mqttSelectService.CreateAllAsync();

var mqttSceneService = host.Services.GetRequiredService<IMqttSceneService>();
await mqttSceneService.CreateAllAsync();